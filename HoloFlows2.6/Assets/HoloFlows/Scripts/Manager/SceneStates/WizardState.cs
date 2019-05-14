using HoloFlows.Devices;
using HoloFlows.ObjectDetection;
using HoloFlows.Wizard;
using UnityEngine;

namespace HoloFlows.Manager.SceneStates
{
    internal class WizardState : AppState
    {
        private GameObject wizard;
        private QRCodeData data;

        public WizardState(HoloFlowSceneManager sceneManager, QRCodeData data) : base(sceneManager)
        {
            this.data = data;
        }

        public void StartWizard()
        {
            EnableProgressIndicator("Loading Workflow...");
            WizardTaskManager.Instance.LoadWorkflowForLastScan(WorkflowReady);
        }

        private void WorkflowReady(bool isReady)
        {
            DisableProgressIndicator();
            wizard = GameObject.Instantiate(PrefabHolder.Instance.assemblyWizard);
            WizardDialog dialog = wizard.GetComponent<WizardDialog>();
            if (dialog != null)
            {
                dialog.WizardDone = WizardDone;
                dialog.LoadFirstTaskAndActivate();
            }
            else { Debug.LogError("There was no WizardDialog found. Callback not set."); }
        }

        private void WizardDone()
        {
            SwitchToEdit();
        }

        public override void SwitchToEdit()
        {
            //TODO destroy wizard here?
            DeviceSpawner.Instance.SpawnDevice(data.ThingId, go =>
            {
                //in case device could not be spawned
                if (go == null)
                {
                    ShowAllManagedObjects();
                    SetNewState(new ControlState(sceneManager));
                    Debug.Log("Switched to ControlState");
                    return;
                }

                //the spawned device is in placing mode
                ShowAllManagedObjects();
                EnablePlacingModeForManagedObjects(true);
                PlaceTheGameObject(go);
                SetNewState(new EditState(sceneManager));
                Debug.Log("Switched to EditState");
            });
        }

        protected override ApplicationState GetApplicationState() { return ApplicationState.Wizard; }
    }
}
