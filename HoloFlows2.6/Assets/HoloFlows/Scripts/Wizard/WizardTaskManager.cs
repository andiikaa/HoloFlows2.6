using HoloFlows.ObjectDetection;
using HoloFlows.Processes;
using HoloToolkit.Unity;
using Processes.Proteus.Rest.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HoloFlows.Wizard
{

    /// <summary>
    /// Handles all Tasks in FIFO Order.
    /// </summary>
    public class WizardTaskManager : Singleton<WizardTaskManager>
    {
        //FIXME remove if not used anymore
        //if set to true, no proteus should be need (for demo)
        //public static bool USE_STATIC_FOR_DEMO = false;

        private QRCodeData qrCodeData;
        private ProteusRestClient proteusRestClient;
        //instance id of the root process
        private string processInstanceId;
        private string processId;
        private IHumanTaskRequest latestRequest;

        #region upload, deploy and start helpers

        List<IProcessInfo> uProcessInfos = null;
        bool? uHasError = null;

        #endregion

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// If a last scanned tag was added, the wizardtaskmanager begins to work...
        /// </summary>
        public void AddLastScannedData(QRCodeData scannedData)
        {
            if (!scannedData.IsValid)
            {
                Debug.LogError("Invalid qr code data added to the WizardTaskManager");
                return;
            }
            proteusRestClient = new ProteusRestClient();
            qrCodeData = scannedData;
        }

        /// <summary>
        /// Start and instantiate the workflow for the last qr code scan
        /// </summary>
        public void LoadWorkflowForLastScan(Action<bool> workflowReady)
        {
            processId = qrCodeData.WorkflowId;
            StartCoroutine(DeployAndStartProcess(processId, workflowReady));
        }

        public void GetNextTask(Action<WizardTask> taskReady)
        {
            StartCoroutine(GetNextTaskInternal(taskReady));
        }

        //TODO error handling if something fails - to bring back the default state and let the user scan again
        private IEnumerator DeployAndStartProcess(string processId, Action<bool> workflowReady)
        {
            uHasError = null;
            uProcessInfos = null;

            yield return GetDeployedProcessList();
            if (uHasError == true) yield break;

            yield return UploadProcessIfNotDeployed();
            if (uHasError == true) yield break;

            yield return DeployProcessInstance();
            if (uHasError == true) yield break;

            yield return proteusRestClient.StartProcessInstance(processInstanceId, isRunning =>
            {
                workflowReady?.Invoke(!isRunning.HasError);
            });
        }

        #region upload, deploy and start methods


        private IEnumerator DeployProcessInstance()
        {
            bool? hasError = null;
            yield return proteusRestClient.DeployProcessInstance(processId, response =>
            {
                processInstanceId = response.Data;
                hasError = response.HasError;
            });

            while (hasError == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            if (hasError == true || processInstanceId == null)
            {
                Debug.LogError("failed to deploy process instance");
            }

            uHasError = hasError;
        }

        private IEnumerator GetDeployedProcessList()
        {
            bool? hasError = null;
            yield return proteusRestClient.GetProcesses(pl =>
            {
                hasError = pl.HasError;
                uProcessInfos = pl.Data;
            });

            while (hasError == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            if (hasError == true || uProcessInfos == null)
            {
                Debug.LogError("failed to get process list from proteus");
            }
            uHasError = hasError;
        }

        private IEnumerator UploadProcessIfNotDeployed()
        {
            bool? hasError = null;
            bool processAlreadyDeployed = uProcessInfos.Any(p => p.ProcessId == processId);
            if (processAlreadyDeployed) yield break;

            string processDoc = ProcessLoadUtil.LoadLocalProcessDefinition(qrCodeData.WorkflowName);
            if (processDoc == null)
            {
                Debug.LogErrorFormat("failed to load local process file: {0}", qrCodeData.WorkflowName);
                uHasError = true;
                yield break;
            }

            UploadAndDeployRequest uploadAndDeployRequest = new UploadAndDeployRequest()
            {
                OverrideExisting = true,
                Processdocument = processDoc
            };

            string proteusProcessId = null;
            yield return proteusRestClient.UploadAndDeployProcess(uploadAndDeployRequest, uploadResponse =>
            {
                proteusProcessId = uploadResponse.Data;
                hasError = uploadResponse.HasError;
            });

            while (hasError == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            if (hasError == true || proteusProcessId != processId)
            {
                Debug.LogError("failed to upload and deploy process");
            }
            uHasError = hasError;
        }

        #endregion


        //TODO error handling and timeout or something would be nice?
        private IEnumerator GetNextTaskInternal(Action<WizardTask> taskReady)
        {
            //send response to proteus
            //request next task
            if (processInstanceId == null)
            {
                //TODO at the moment no chance to tell the wizard that an error had happened
                Debug.LogError("no workflow ready?");
                taskReady(null);
                yield break;
            }


            //send a simple response so that the next step can start
            if (latestRequest != null)
            {
                IHumanTaskResponse response = HumanTaskResponseUtil.CreateResponseFromRequest(latestRequest);
                yield return proteusRestClient.PostHumanTaskResponse(response, r =>
                {
                    Debug.Log("response send");
                });
            }

            //short wait
            yield return new WaitForSeconds(1);
            StateEnum? state = null;
            IHumanTaskRequest possibleRequest = null;

            //wait for next task or finish
            while (possibleRequest == null)
            {
                yield return proteusRestClient.GetRecentStateForProcessInstance(processInstanceId, s => { state = s.Data.State; });

                if (state == StateEnum.EXECUTED || state == StateEnum.FAILED)
                {
                    Debug.LogFormat("Workflow done: {0}", processInstanceId);
                    Cleanup();
                    taskReady(null);
                    yield break;
                }

                yield return proteusRestClient.GetHumanTaskListForProcess(processId, l =>
                {
                    if (l.Data != null && l.Data.Count > 0)
                    {
                        possibleRequest = l.Data.Values.First();
                    }
                });
                if (possibleRequest == null) { yield return new WaitForSeconds(0.1f); }
            }

            latestRequest = possibleRequest;
            WizardTask task = CreateWizardTaskFrom(latestRequest);
            taskReady(task);
        }

        private void Cleanup()
        {
            proteusRestClient = null;
            processInstanceId = null;
            processId = null;
            latestRequest = null;
            qrCodeData = null;
            uHasError = null;
            uProcessInfos = null;
        }

        private static WizardTask CreateWizardTaskFrom(IHumanTaskRequest request)
        {
            WizardTask wTask = new WizardTask();
            wTask.Name = request.Name;
            wTask.Instruction = request.Description;
            List<IJSONDataPortInstance> startPorts = request.StartDataPorts.Values.ToList();
            wTask.AudioUri = GetDataPortValue("audioUri", startPorts);
            wTask.ImageUri = GetDataPortValue("imageUri", startPorts);
            return wTask;
        }

        /// <summary>
        /// gets string values from data ports
        /// </summary>
        private static string GetDataPortValue(string portName, List<IJSONDataPortInstance> dataPorts)
        {
            IJSONDataPortInstance port = dataPorts.Where(p => p.Name == portName).FirstOrDefault();
            if (port == null)
            {
                Debug.LogErrorFormat("HumanTask has no data port with name '{0}'", portName);
                return null;
            }
            if (port.DataTypeInstance == null
                || port.DataTypeInstance.DataTypeInstanceType != DataTypeInstanceTypeEnum.StringTypeInstance)
            {
                Debug.LogErrorFormat("HumanTask data port '{0}' has no string value", portName);
                return null;
            }

            return ((IJSONStringTypeInstance)port.DataTypeInstance).Value;
        }

        //private void LoadWorkflowForLastScanStatic(Action<bool> workflowReady)
        //{
        //    tasks.Clear();

        //    //TODO remove hardcoded

        //    //send request
        //    //wait for first human task
        //    //add content
        //    if (qrCodeData.ThingId == "hue_bulb210_1")
        //    {
        //        tasks.AddRange(WizardDemo.CreateCompleteBulbExample());
        //    }
        //    else if (qrCodeData.ThingId == "tinkerforge_irTemp_1")
        //    {
        //        tasks.AddRange(WizardDemo.CreateTinkerforgeIRExample());
        //    }
        //    else
        //    {
        //        tasks.AddRange(WizardDemo.CreateTinkerforgeAmbientLight());
        //    }

        //    workflowReady?.Invoke(true);
        //}

    }
}
