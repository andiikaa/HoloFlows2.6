using HoloFlows.Devices;
using HoloFlows.ObjectDetection;
using HoloToolkit.UX.Progress;
using UnityEngine;

namespace HoloFlows.Manager.SceneStates
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

        /// <summary>
        /// sets the new application state
        /// </summary>
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

        /// <summary>
        /// Enable the progress monitor with the given text
        /// </summary>
        protected void EnableProgressIndicator(string progressText)
        {
            ProgressIndicator.Instance.Open(
                IndicatorStyleEnum.AnimatedOrbs,
                ProgressStyleEnum.None,
                ProgressMessageStyleEnum.Visible, progressText);
        }

        /// <summary>
        /// disable the current progress monitor
        /// </summary>
        protected void DisableProgressIndicator() { ProgressIndicator.Instance.Close(); }

    }
}
