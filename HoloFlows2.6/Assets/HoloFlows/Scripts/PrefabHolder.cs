using HoloToolkit.Unity;
using UnityEngine;

public class PrefabHolder : Singleton<PrefabHolder>
{
    /// <summary>
    /// Define a Wizard, which should be opened to guide a user through the install process of an iot device.
    /// </summary>
    public GameObject assemblyWizard;

    /// <summary>
    /// The default device button for button commands like "ON", "OFF"
    /// </summary>
    public GameObject defaultDeviceButton;

}
