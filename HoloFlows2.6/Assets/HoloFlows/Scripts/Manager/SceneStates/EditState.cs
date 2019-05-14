
using UnityEngine;

namespace HoloFlows.Manager.SceneStates
{
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
