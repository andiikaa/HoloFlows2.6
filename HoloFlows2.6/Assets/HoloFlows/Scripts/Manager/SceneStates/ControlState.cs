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
            SetNewState(new EditState(sceneManager));
            PlayTransitionSound();
            Debug.Log("Switched to Edit Mode");
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

            //check if in editor, cause the cam of the pc is used, 
            //which wont work with the qr scan in most cases
            //if (Application.isEditor)
            //{
            //    JumpToWizard();
            //}
            //else
            //{
            //    StartQrScanning();
            //}
            StartQrScanning();
        }

        private void StartQrScanning()
        {
            //enable scan interface and set fixed position to camera
            GameObject cameraObject = GameObject.Find(CAM_NAME);
            if (cameraObject == null) { throw new System.NullReferenceException(string.Format(ERR_CAM_MSG, CAM_NAME)); }
            Transform scanInterface = cameraObject.transform.Find(SCAN_NAME);
            if (scanInterface == null) { throw new System.NullReferenceException(string.Format(ERR_CAM_MSG, SCAN_NAME)); }

            QRCodeScanner scanner = scanInterface.GetComponent<QRCodeScanner>();
            if (scanner == null) { throw new System.NullReferenceException("QrCodeScanner behavior not found"); }

            //check if in editor, cause the cam of the pc is used, 
            //which wont work with the qr scan in most cases
            if (!Application.isEditor)
            {
                scanner.InitDefaults();
                scanInterface.gameObject.SetActive(true);
            }

            SetNewState(new QRScanState(sceneManager, scanInterface.gameObject));

            if (Application.isEditor)
            {
                sceneManager.SwitchToWizard(QRCodeData.ForDebug());
            }

            Debug.Log("Switched to QRScanState");
        }

        private void JumpToWizard()
        {
            //debug data cause camera will not work in editor
            QRCodeData data = QRCodeData.ForDebug();
            WizardTaskManager.Instance.AddLastScannedData(data);
            WizardState wState = new WizardState(sceneManager, data);
            SetNewState(wState);
            wState.StartWizard();
            Debug.Log("Switched to WizardState");
        }

        protected override ApplicationState GetApplicationState() { return ApplicationState.Control; }
    }
}
