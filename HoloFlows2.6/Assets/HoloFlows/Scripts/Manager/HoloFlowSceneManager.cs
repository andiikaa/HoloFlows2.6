using HoloToolkit.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using static HoloFlows.Manager.HoloFlowSceneManager;

namespace HoloFlows.Manager
{
    /// <summary>
    /// Handles the visibilty for all application states.
    /// </summary>
    public class HoloFlowSceneManager : Singleton<HoloFlowSceneManager>, IApplicationStateManager
    {
        private const string ERR_STATE_MSG = "current state does not support the switch to '{0}' state";
        private const string ERR_CAM_MSG = "a camera game object with name '{0}' could not be found in the active scene";
        private const string CAM_NAME = "HoloLensCamera";

        private HashSet<ManagedObject> managedObjects = new HashSet<ManagedObject>();
        private AppState internalState;

        // Use this for initialization
        void Start()
        {
            internalState = new ControlState(this);
        }


        /// <summary>
        /// Registers a <see cref="ManagedObject"/> which should be managed by the <see cref="HoloFlowSceneManager"/>
        /// </summary>
        public void RegisterObject(ManagedObject mObject)
        {
            managedObjects.Add(mObject);
        }

        public void UnregisterObject(ManagedObject mObject)
        {
            managedObjects.Remove(mObject);
        }

        #region State Switches
        public void SwitchToQRScan() { internalState.SwitchToQRScan(); }
        public void SwitchToEdit() { internalState.SwitchToEdit(); }
        public void SwitchToControl() { internalState.SwitchToControl(); }
        public void SwitchToWizard() { internalState.SwitchToWizard(); }
        #endregion

        #region AppStates

        private abstract class AppState : IApplicationStateManager
        {
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
                foreach (ManagedObject mObj in sceneManager.managedObjects)
                {
                    mObj.Hide();
                }
            }

            protected void EnablePlacingModeForManagedObjects(bool enable)
            {
                foreach (ManagedObject mObj in sceneManager.managedObjects)
                {
                    mObj.EnablePlacingBox(enable);
                }
            }

            protected void SetNewState(AppState state)
            {
                sceneManager.internalState = state;
            }

        }

        private class ControlState : AppState
        {
            public ControlState(HoloFlowSceneManager sceneManager) : base(sceneManager) { }

            public override void SwitchToEdit()
            {
                EnablePlacingModeForManagedObjects(true);
                sceneManager.internalState = new EditState(sceneManager);
            }

            //QRScan hides all devices, inits the image capture and enables the scan interface
            public override void SwitchToQRScan()
            {
                //hide all
                HideAllManagedObjects();

                //enable scan interface and set fixed position to camera
                GameObject scanInterface = Instantiate(PrefabHolder.Instance.qrScanInterface);
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

        private class QRScanState : AppState
        {
            private GameObject scanInterface;

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
                Destroy(scanInterface);
                SetNewState(new WizardState(sceneManager));
            }

        }

        private class WizardState : AppState
        {
            public WizardState(HoloFlowSceneManager sceneManager) : base(sceneManager) { }

            public override void SwitchToEdit()
            {
                //TODO destroy wizard
                EnablePlacingModeForManagedObjects(true);
                SetNewState(new EditState(sceneManager));
            }
        }

        private class EditState : AppState
        {
            public EditState(HoloFlowSceneManager sceneManager) : base(sceneManager) { }
        }

        public interface IApplicationStateManager
        {
            void SwitchToQRScan();
            void SwitchToEdit();
            void SwitchToControl();
            void SwitchToWizard();
        }

        #endregion
    }

}
