

using System;
using UnityEngine;

namespace HoloFlows.Manager
{
    public abstract class AppState : IApplicationStateManager
    {

        protected const string ERR_STATE_MSG = "current state does not support the switch to '{0}' state";
        protected const string ERR_CAM_MSG = "a camera game object with name '{0}' could not be found in the active scene";
        protected const string CAM_NAME = "HoloLensCamera";

        protected HoloFlowSceneManager sceneManager;

        public AppState(HoloFlowSceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public virtual void SwitchToControl() { Debug.LogWarningFormat(ERR_STATE_MSG, "control"); }
        public virtual void SwitchToEdit() { Debug.LogWarningFormat(ERR_STATE_MSG, "edit"); }
        public virtual void SwitchToQRScan() { Debug.LogWarningFormat(ERR_STATE_MSG, "qr_scan"); }
        public virtual void SwitchToWizard() { Debug.LogWarningFormat(ERR_STATE_MSG, "wizard"); }

        protected void HideAllManagedObjects()
        {
            foreach (IManagedObject mObj in sceneManager.ManagedObjects)
            {
                mObj.Hide();
            }
        }

        protected void EnablePlacingModeForManagedObjects(bool enable)
        {
            foreach (IManagedObject mObj in sceneManager.ManagedObjects)
            {
                mObj.EnablePlacingBox(enable);
            }
        }

        protected void SetNewState(AppState state)
        {
            sceneManager.InternalState = state;
        }

    }

    public class ControlState : AppState
    {
        public ControlState(HoloFlowSceneManager sceneManager) : base(sceneManager) { }

        public override void SwitchToEdit()
        {
            EnablePlacingModeForManagedObjects(true);
            sceneManager.InternalState = new EditState(sceneManager);
        }

        //QRScan hides all devices, inits the image capture and enables the scan interface
        public override void SwitchToQRScan()
        {
            //hide all
            HideAllManagedObjects();

            //enable scan interface and set fixed position to camera
            GameObject scanInterface = sceneManager.InternalInstantiate(PrefabHolder.Instance.qrScanInterface);
            GameObject cameraObject = GameObject.Find(CAM_NAME);
            if (cameraObject == null)
            {
                throw new NullReferenceException(string.Format(ERR_CAM_MSG, CAM_NAME));
            }

            scanInterface.transform.SetParent(cameraObject.transform);
            scanInterface.SetActive(true);

            SetNewState(new QRScanState(sceneManager, scanInterface));
        }
    }

    public class QRScanState : AppState
    {
        private readonly GameObject scanInterface;

        public QRScanState(HoloFlowSceneManager sceneManager, GameObject scanInterface) : base(sceneManager)
        {
            if (scanInterface == null)
            {
                throw new NullReferenceException("scanInterface could not be null for QRScanState");
            }

            this.scanInterface = scanInterface;
        }

        public override void SwitchToWizard()
        {
            sceneManager.InternalDestroy(scanInterface);
            SetNewState(new WizardState(sceneManager));
        }

    }

    public class WizardState : AppState
    {
        public WizardState(HoloFlowSceneManager sceneManager) : base(sceneManager) { }

        public override void SwitchToEdit()
        {
            //TODO destroy wizard
            EnablePlacingModeForManagedObjects(true);
            SetNewState(new EditState(sceneManager));
        }
    }

    public class EditState : AppState
    {
        public EditState(HoloFlowSceneManager sceneManager) : base(sceneManager) { }
    }


}
