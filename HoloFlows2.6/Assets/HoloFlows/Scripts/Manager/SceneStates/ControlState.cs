using HoloFlows.ObjectDetection;
using HoloFlows.Wizard;
using UnityEngine;

namespace HoloFlows.Manager.SceneStates
{
    internal class ControlState : AppState
    {
        public ControlState(HoloFlowSceneManager sceneManager) : base(sceneManager) { }

        public override void SwitchToEdit()
        {
            EnablePlacingModeForManagedObjects(true);
            sceneManager.InternalState = new EditState(sceneManager);
            PlayTransitionSound();
            Debug.Log("Switched to QRScanState");
        }

        protected override AudioSource GetTransitionSound()
        {
            return AudioLibrary.Instance.SwitchFromControlToEdit;
        }

        //QRScan hides all devices, inits the image capture and enables the scan interface
        public override void SwitchToQRScan()
        {
            //hide all
            HideAllManagedObjects();

            //TODO remove direct jump to wizard

            //enable scan interface and set fixed position to camera
            //GameObject cameraObject = GameObject.Find(CAM_NAME);
            //if (cameraObject == null) { throw new NullReferenceException(string.Format(ERR_CAM_MSG, CAM_NAME)); }
            //Transform scanInterface = cameraObject.transform.Find(SCAN_NAME);
            //if (scanInterface == null) { throw new NullReferenceException(string.Format(ERR_CAM_MSG, SCAN_NAME)); }

            //scanInterface.gameObject.SetActive(true);

            //SetNewState(new QRScanState(sceneManager, scanInterface.gameObject));
            //Debug.Log("Switched to QRScanState");
            JumpToWizard();
        }

        private void JumpToWizard()
        {
            //TODO ftom qr code data
            QRCodeData data = QRCodeData.FromQrCodeData("FIXMEFIXME");
            WizardTaskManager.Instance.AddLastScannedData(data);
            WizardState wState = new WizardState(sceneManager, data);
            SetNewState(wState);
            wState.StartWizard();
            Debug.Log("Switched to WizardState");
        }

        protected override ApplicationState GetApplicationState() { return ApplicationState.Control; }
    }
}
