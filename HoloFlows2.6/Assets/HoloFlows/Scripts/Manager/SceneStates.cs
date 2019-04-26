

using HoloFlows.Devices;
using HoloFlows.ObjectDetection;
using HoloFlows.Wizard;
using System;
using UnityEngine;

namespace HoloFlows.Manager
{
    /// <summary>
    /// Application states which are handled by the <see cref="HoloFlowSceneManager"/>
    /// </summary>
    internal abstract class AppState : IApplicationStateManager
    {
        protected const string ERR_STATE_MSG = "current state does not support the switch to '{0}' state";
        protected const string ERR_CAM_MSG = "a game object with name '{0}' could not be found in the active scene";
        protected const string CAM_NAME = "HoloLensCamera";
        protected const string SCAN_NAME = "QRCodeScanner";

        protected HoloFlowSceneManager sceneManager;

        public ApplicationState ApplicationState
        {
            get
            {
                return GetApplicationState();
            }
        }

        public AppState(HoloFlowSceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public virtual void SwitchToControl() { Debug.LogWarningFormat(ERR_STATE_MSG, "control"); }
        public virtual void SwitchToEdit() { Debug.LogWarningFormat(ERR_STATE_MSG, "edit"); }
        public virtual void SwitchToQRScan() { Debug.LogWarningFormat(ERR_STATE_MSG, "qr_scan"); }
        public virtual void SwitchToWizard(QRCodeData data) { Debug.LogWarningFormat(ERR_STATE_MSG, "wizard"); }
        protected virtual AudioSource GetTransitionSound() { return null; }
        protected abstract ApplicationState GetApplicationState();

        protected void HideAllManagedObjects()
        {
            foreach (IManagedObject mObj in sceneManager.ManagedObjects)
            {
                mObj.Hide();
            }
        }

        protected void ShowAllManagedObjects()
        {
            foreach (IManagedObject mObj in sceneManager.ManagedObjects)
            {
                mObj.Show();
            }
        }

        protected void EnablePlacingModeForManagedObjects(bool enable)
        {
            foreach (IManagedObject mObj in sceneManager.ManagedObjects)
            {
                mObj.EnablePlacingBox(enable);
            }
        }

        /// <summary>
        /// Starts the placing for the game object.
        /// </summary>
        /// <param name="go"></param>
        protected void PlaceTheGameObject(GameObject go)
        {
            DeviceBehaviorBase db = go.GetComponent<DeviceBehaviorBase>();
            if (db == null)
            {
                Debug.LogErrorFormat("could not find DeviceBehaviorBase for {0}", go.name);
                return;
            }
            db.StartPlacing();
        }

        protected void SetNewState(AppState state)
        {
            sceneManager.InternalState = state;
        }

        /// <summary>
        /// ensures that audio library is initialized and audio source is set
        /// </summary>
        protected void PlayTransitionSound()
        {
            if (AudioLibrary.IsInitialized)
            {
                AudioSource soundToPlay = GetTransitionSound();
                soundToPlay?.Play();
            }
            else
            {
                Debug.LogError("AudioLibrary is not initialized or audio source not set");
            }
        }

    }

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
            QRCodeData data = QRCodeData.FromQrCodeData("FIXMEFIXME");
            WizardTaskManager.Instance.AddLastScannedData(data);
            WizardState wState = new WizardState(sceneManager, data);
            SetNewState(wState);
            wState.StartWizard();
            Debug.Log("Switched to WizardState");
        }

        protected override ApplicationState GetApplicationState() { return ApplicationState.Control; }
    }

    internal class QRScanState : AppState
    {
        private readonly GameObject scanInterface;

        public QRScanState(HoloFlowSceneManager sceneManager, GameObject scanInterface) : base(sceneManager)
        {
            if (scanInterface == null)
            {
                throw new ArgumentNullException("scanInterface could not be null for QRScanState");
            }

            this.scanInterface = scanInterface;
        }

        public override void SwitchToWizard(QRCodeData data)
        {
            //sceneManager.InternalDestroy(scanInterface);
            scanInterface.SetActive(false);
            if (!data.IsValid)
            {
                SetNewState(new ControlState(sceneManager));
                Debug.Log("Switched to ControlState");
                return;
            }

            WizardTaskManager.Instance.AddLastScannedData(data);
            WizardState wState = new WizardState(sceneManager, data);
            SetNewState(wState);
            wState.StartWizard();
            Debug.Log("Switched to WizardState");
        }

        protected override ApplicationState GetApplicationState() { return ApplicationState.QRScan; }
    }

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
            wizard = GameObject.Instantiate(PrefabHolder.Instance.assemblyWizard);
            WizardDialog dialog = wizard.GetComponent<WizardDialog>();
            if (dialog != null) { dialog.WizardDone = WizardDone; }
            else { Debug.LogError("There was no WizardDialog found. Callback not set."); }
            wizard.SetActive(true);
        }

        private void WizardDone()
        {
            SwitchToEdit();
        }

        public override void SwitchToEdit()
        {
            //TODO destroy wizard here?
            GameObject go = DeviceSpawner.Instance.SpawnDevice(data.ThingId);

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
        }

        protected override ApplicationState GetApplicationState() { return ApplicationState.Wizard; }
    }

    internal class EditState : AppState
    {
        public EditState(HoloFlowSceneManager sceneManager) : base(sceneManager) { }

        public override void SwitchToControl()
        {
            EnablePlacingModeForManagedObjects(false);
            SetNewState(new ControlState(sceneManager));
            PlayTransitionSound();
            Debug.Log("Switched to ControlState");
        }

        protected override AudioSource GetTransitionSound()
        {
            return AudioLibrary.Instance.SwitchFromEditToControl;
        }

        protected override ApplicationState GetApplicationState() { return ApplicationState.Edit; }
    }

}
