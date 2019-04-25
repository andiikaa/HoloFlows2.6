using HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace HoloFlows.Manager
{
    /// <summary>
    /// Handles the visibilty for <see cref="IManagedObject"/>s for all application states <see cref="AppState"/>.
    /// </summary>
    public class HoloFlowSceneManager : Singleton<HoloFlowSceneManager>, IApplicationStateManager
    {
        internal HashSet<IManagedObject> ManagedObjects { get; set; } = new HashSet<IManagedObject>();
        internal AppState InternalState { get; set; }

        // Use this for initialization
        void Start()
        {
            InternalState = new ControlState(this);
        }


        /// <summary>
        /// Registers a <see cref="IManagedObject"/> which should be managed by the <see cref="HoloFlowSceneManager"/>
        /// </summary>
        public void RegisterObject(IManagedObject mObject)
        {
            ManagedObjects.Add(mObject);
        }

        public void UnregisterObject(IManagedObject mObject)
        {
            ManagedObjects.Remove(mObject);
        }

        internal GameObject InternalInstantiate(GameObject gameObject)
        {
            return Instantiate(gameObject);
        }

        internal void InternalDestroy(GameObject gameObject)
        {
            Destroy(gameObject);
        }

        #region State Switches
        public void SwitchToQRScan() { InternalState.SwitchToQRScan(); }
        public void SwitchToEdit() { InternalState.SwitchToEdit(); }
        public void SwitchToControl() { InternalState.SwitchToControl(); }
        public void SwitchToWizard() { InternalState.SwitchToWizard(); }
        #endregion

    }

    public interface IApplicationStateManager
    {
        void SwitchToQRScan();
        void SwitchToEdit();
        void SwitchToControl();
        void SwitchToWizard();
    }

}
