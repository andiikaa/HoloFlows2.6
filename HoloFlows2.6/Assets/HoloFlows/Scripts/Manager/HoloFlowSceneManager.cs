using HoloToolkit.Unity;
using System.Collections.Generic;
using static HoloFlows.Manager.HoloFlowSceneManager;

namespace HoloFlows.Manager
{
    /// <summary>
    /// Handles the visibilty for all application states.
    /// </summary>
    public class HoloFlowSceneManager : Singleton<DeviceManager>, ApplicationStateManager
    {
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

        private abstract class AppState : ApplicationStateManager
        {
            protected HoloFlowSceneManager sceneManager;

            public AppState(HoloFlowSceneManager sceneManager)
            {
                this.sceneManager = sceneManager;
            }

            public virtual void SwitchToControl() { }
            public virtual void SwitchToEdit() { }
            public virtual void SwitchToQRScan() { }
            public virtual void SwitchToWizard() { }
        }

        private class ControlState : AppState
        {
            public ControlState(HoloFlowSceneManager sceneManager) : base(sceneManager) { }

            public override void SwitchToEdit()
            {
                foreach (ManagedObject mObj in sceneManager.managedObjects)
                {
                    mObj.Hide();
                }
                sceneManager.internalState = new EditState(sceneManager);
            }
        }

        private class EditState : AppState
        {
            public EditState(HoloFlowSceneManager sceneManager) : base(sceneManager) { }
        }

        public interface ApplicationStateManager
        {
            void SwitchToQRScan();
            void SwitchToEdit();
            void SwitchToControl();
            void SwitchToWizard();
        }

        #endregion
    }

}
