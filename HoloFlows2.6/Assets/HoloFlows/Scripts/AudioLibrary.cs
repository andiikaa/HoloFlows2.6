using HoloToolkit.Unity;
using UnityEngine;

namespace HoloFlows
{
    public class AudioLibrary : Singleton<AudioLibrary>
    {
        public AudioSource ButtonAirTap;

        public AudioSource SwitchFromControlToEdit;

        public AudioSource SwitchFromEditToControl;

    }
}
