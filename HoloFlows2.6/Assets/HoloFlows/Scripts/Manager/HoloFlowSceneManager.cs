using HoloFlows.Manager.SceneStates;
using HoloFlows.ObjectDetection;
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
        internal HashSet<IManagedObject> ManagedObjects { get; private set; } = new HashSet<IManagedObject>();
        internal AppState InternalState { get; set; }

        /// <summary>
        /// Gets the current <see cref="ApplicationState"/>.
        /// </summary>
        public ApplicationState ApplicationState
        {
            get { return InternalState.ApplicationState; }
        }

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

        /// <summary>
        /// Unregister the <see cref="IManagedObject"/>. The object is then no longer managed by the <see cref="HoloFlowSceneManager"/>
        /// </summary>
        /// <param name="mObject"></param>
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
        public void SwitchToWizard(QRCodeData data) { InternalState.SwitchToWizard(data); }
        #endregion

    }

    public interface IApplicationStateManager
    {
        void SwitchToQRScan();
        void SwitchToEdit();
        void SwitchToControl();
        void SwitchToWizard(QRCodeData data);
        ApplicationState ApplicationState { get; }

    }

    public enum ApplicationState
    {
        QRScan, Edit, Control, Wizard
    }

}
