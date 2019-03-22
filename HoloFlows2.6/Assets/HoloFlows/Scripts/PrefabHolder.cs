using HoloToolkit.Unity;
using UnityEngine;

public class PrefabHolder : Singleton<PrefabHolder>
{
    /// <summary>
    /// Define a Wizard, which should be opened to guide a user through the install process of an iot device.
    /// </summary>
    public GameObject assemblyWizard;

    /// <summary>
    /// Holds all device specific prefabs
    /// </summary>
    [Tooltip("Holds all device specific prefabs")]
    public Devices devices;


    private void Start()
    {
        ValidateSettings();
    }

    #region grouping

    [System.Serializable]
    public class Devices
    {
        /// <summary>
        /// The default device button for button commands like "ON", "OFF"
        /// </summary>
        [Tooltip("The default device button for button commands like 'ON', 'OFF'")]
        public GameObject defaultDeviceButton;

        /// <summary>
        /// Prefab for basic devices.
        /// </summary>
        [Tooltip("Prefab for basic devices")]
        public GameObject basicDevice;

        /// <summary>
        /// Prefab for devices with 2 billboards
        /// </summary>
        [Tooltip("Prefab for devices with 2 billboards")]
        public GameObject twoWayDevice;

        /// <summary>
        /// Prefab for devices with 3 billboards
        /// </summary>
        [Tooltip("Prefab for devices with 3 billboards")]
        public GameObject threeWayDevice;

        /// <summary>
        /// Prefab for devices with 4 or more billboards
        /// </summary>
        [Tooltip("Prefab for devices with 4 or more billboards")]
        public GameObject multiDevice;

        /// <summary>
        /// name for the button layout group name in prefabs
        /// </summary>
        [Tooltip("Name for the button layout group name in prefabs")]
        public string buttonLayoutGroupName;

        /// <summary>
        /// Button group which contains (nice arranged) on, off, up and down.
        /// </summary>
        [Tooltip("Button group which contains (nice arranged) on, off, up and down.")]
        public GameObject onOffUpDownButton;
    }

    #endregion

    #region validation

    private void ValidateSettings()
    {
        CheckNull(assemblyWizard, "assemblyWizard");
        CheckNull(devices, "devices");
        CheckDevices();
    }

    private void CheckDevices()
    {
        if (devices == null)
        {
            return;
        }

        CheckNull(devices.defaultDeviceButton, "devices.defaultDeviceButton");
        CheckNull(devices.basicDevice, "devices.basicDevice");
        CheckNull(devices.twoWayDevice, "devices.twoWayDevice");
        CheckNull(devices.threeWayDevice, "devices.threeWayDevice");
        CheckNull(devices.multiDevice, "devices.multiDevice");
        CheckNull(devices.buttonLayoutGroupName, "devices.buttonLayoutGroupName");
    }

    private void CheckNull(object obj, string name)
    {
        if (obj == null)
        {
            Debug.LogWarningFormat("There is no value set for '{0}'", name);
        }
    }

    #endregion

}
