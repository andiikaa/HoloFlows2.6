using HoloToolkit.Unity.InputModule;
using UnityEngine;


namespace HoloFlows.ButtonScripts
{

    /// <summary>
    /// Base class for button behaviors with the air tap sound.
    /// </summary>
    public abstract class TapSoundButton : MonoBehaviour, IInputClickHandler
    {

        private AudioSource airTapAudio;

        /// <summary>
        /// Base class starting routine. If overriden you must call this method explicit.
        /// </summary>
        public virtual void Start()
        {
            if (!AudioLibrary.IsInitialized)
            {
                Debug.LogError("AudioLibrary is not initialized");
                return;
            }

            airTapAudio = AudioLibrary.Instance.ButtonAirTap;
        }

        /// <summary>
        /// Handles the click, playes the sound and redirects the event to <see cref="HandleClickEvent(InputClickedEventData)"/> method.
        /// Dont override this method! Handle the click event in <see cref="HandleClickEvent(InputClickedEventData)"/>.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnInputClicked(InputClickedEventData eventData)
        {
            if (airTapAudio != null)
            {
                airTapAudio.Play();
            }
            else
            {
                Debug.LogError("AirTapAudioSource not set!");
            }

            HandleClickEvent(eventData);
        }

        /// <summary>
        /// Method which handles the click event must be implemented by all subclasses.
        /// </summary>
        /// <param name="eventData"></param>
        public abstract void HandleClickEvent(InputClickedEventData eventData);
    }
}
