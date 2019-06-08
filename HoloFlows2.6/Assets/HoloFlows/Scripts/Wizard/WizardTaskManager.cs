using HoloFlows.ObjectDetection;
using HoloFlows.Processes;
using HoloToolkit.Unity;
using Processes.Proteus.Rest.Model;
using System;
using System.Collections;
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
            //FIXME if possible 
            proteusRestClient = new ProteusRestClient("http://127.0.0.1:8082");
            qrCodeData = scannedData;
        }

        /// <summary>
        /// Start and instantiate the workflow for the last qr code scan
        /// </summary>
        public void LoadWorkflowForLastScan(Action<bool> workflowReady)
        {
            //FIXME get this from the qrcode
            processId = "_17j-IIloEem6b9zSoTSELQ"; //simple wizard
            StartCoroutine(DeployAndStartProcess(processId, workflowReady));
        }

        private IEnumerator DeployAndStartProcess(string processId, Action<bool> workflowReady)
        {
            //TODO check if process is deployed on proteus
            //if process is not deployed, deploy from remote store

            yield return proteusRestClient.DeployProcessInstance(processId, response =>
            {
                processInstanceId = response.Data;
            });

            yield return new WaitForSeconds(2);

            yield return proteusRestClient.StartProcessInstance(processInstanceId, isRunning =>
            {
                workflowReady?.Invoke(!isRunning.HasError);
            });
        }

        public void GetNextTask(Action<WizardTask> taskReady)
        {
            StartCoroutine(GetNextTaskInternal(taskReady));
        }

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
        }

        private static WizardTask CreateWizardTaskFrom(IHumanTaskRequest request)
        {
            WizardTask wTask = new WizardTask();
            wTask.Name = request.Name;
            wTask.Instruction = request.Description;
            return wTask;
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
