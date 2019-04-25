using HoloFlows.ButtonScripts;
using HoloFlows.Manager;
using HoloFlows.Model;
using HoloToolkit.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HoloFlows.Devices
{
    public class DeviceSpawner : Singleton<DeviceSpawner>
    {
        private DeviceManager deviceManager;

        // Use this for initialization
        void Start()
        {

        }

        public void SpawnDevice(string deviceUid)
        {
            //Get The DeviceManager for a bit shorter syntax
            if (deviceManager == null)
            {
                deviceManager = DeviceManager.Instance;
            }

            //DeviceInfo info = deviceManager.DeviceInfos["homematic_dimmer_1"];
            //GameObject go = GetDeviceSpecificPrefab(info);
            //if (go == null)
            //{
            //    Debug.LogErrorFormat("failed to spawn device for uid '{0}'", deviceUid);
            //}

            //tinkerforge_irTemp_1
            DeviceInfo info2 = deviceManager.DeviceInfos["homematic_dimmer_1"];
            GameObject go2 = GetDeviceSpecificPrefab(info2);
            if (go2 == null)
            {
                Debug.LogErrorFormat("failed to spawn device for uid '{0}'", deviceUid);
            }
        }

        public GameObject MergeBasicDevices(GameObject device1, GameObject device2)
        {
            if (!device1.name.StartsWith("BasicDevice") || !device2.name.StartsWith("BasicDevice"))
            {
                Debug.LogError("At least on of the merging devices is not from type BasicDevice");
                return null;
            }

            GameObject go = Instantiate(PrefabHolder.Instance.devices.twoWayDevice);
            TwoWayDevice mergedDevice = go.GetComponent<TwoWayDevice>();
            mergedDevice.CopyFromBasicDevices(device1, device2);
            go.SetActive(true);
            return go;
        }

        //get the prefab specific for the device (Basic, 2Way, 3Way, Multi)
        private GameObject GetDeviceSpecificPrefab(DeviceInfo info)
        {
            //TODO implement all cases
            if (info.States != null && info.States.Any())
            {
                //get all itemids from the state and group them
                //the group counter should give the different devices
                IEnumerable<IGrouping<string, string>> groups = info.States.
                    Select(e => e.ItemId).
                    GroupBy(e => e);

                int differentGroups = groups.Count();

                if (differentGroups == 1)
                {
                    return InstantiateBasicDevice(info);
                }
                else if (differentGroups == 2)
                {
                    return InstantiateTwoWayDevice(info, groups);
                }
                else if (differentGroups == 3)
                {
                    return InstantiateThreeWayDevice(info);
                }
                else
                {
                    Debug.LogError("Implement MultiDevice!");
                    return null;
                }
            }

            return InstantiateBasicDevice(info);
        }

        private GameObject InstantiateTwoWayDevice(DeviceInfo info, IEnumerable<IGrouping<string, string>> groups)
        {
            //TODO we must find the specific buttons, which control the state. 
            //But what to do if there is no state? How to find a name?
            GameObject go = Instantiate(PrefabHolder.Instance.devices.twoWayDevice);
            TwoWayDevice device = go.GetComponent<TwoWayDevice>();
            device.SetDeviceInfos(info);
            ActivateGameObjectIfNeeded(go);
            return go;
        }

        private void ActivateGameObjectIfNeeded(GameObject go)
        {
            if (go != null && !go.activeSelf)
            {
                go.SetActive(true);
            }
        }

        private GameObject InstantiateThreeWayDevice(DeviceInfo info)
        {
            GameObject go = Instantiate(PrefabHolder.Instance.devices.threeWayDevice);
            ThreeWayDevice device = go.GetComponent<ThreeWayDevice>();
            device.SetDeviceInfos(info);
            ActivateGameObjectIfNeeded(go);
            return go;
        }

        private GameObject InstantiateBasicDevice(DeviceInfo info)
        {
            GameObject go = Instantiate(PrefabHolder.Instance.devices.basicDevice);
            Transform btnLayoutGroup = go.transform.Find(PrefabHolder.Instance.devices.buttonLayoutGroupName);
            if (!AddButtonsToLayoutGroup(btnLayoutGroup, info))
            {
                Debug.LogError("failed to find the layout group for the buttons");
                return null;
            }
            ActivateGameObjectIfNeeded(go);
            return go;
        }

        private bool AddButtonsToLayoutGroup(Transform layoutGroup, DeviceInfo info)
        {
            if (layoutGroup == null || layoutGroup.GetComponent<VerticalLayoutGroup>() == null)
                return false;

            List<DeviceFunctionality> onOffUpDownFunc = GetOffUpDownFunctionalities(info);
            if (onOffUpDownFunc.Any())
            {
                AddOnOffUpDownCombination(layoutGroup, onOffUpDownFunc);
            }
            return true;
        }

        private void AddOnOffUpDownCombination(Transform layoutGroup, List<DeviceFunctionality> onOffUpDownFunc)
        {
            if (onOffUpDownFunc.Count != 1)
            {
                Debug.LogError("Basic Device supports only one device and more than one onOffUpDown functionalities are not supported");
            }

            DeviceFunctionality func = onOffUpDownFunc.First();
            DeviceCommand onCommand = func.Commands.First(c => TYPE_ON_COMMAND == c.CommandType);
            DeviceCommand offCommand = func.Commands.First(c => TYPE_OFF_COMMAND == c.CommandType);
            DeviceCommand upCommand = func.Commands.First(c => TYPE_UP_COMMAND == c.CommandType);
            DeviceCommand downCommand = func.Commands.First(c => TYPE_DOWN_COMMAND == c.CommandType);

            //buttons are visible in this order from top to bottom
            GameObject btnObject = Instantiate(PrefabHolder.Instance.devices.onOffUpDownButton);
            OnOffUpDownButton btnScript = btnObject.GetComponent<OnOffUpDownButton>();
            btnScript.SetDownButtonData(func.ItemId, downCommand.RealCommandName);
            btnScript.SetUpButtonData(func.ItemId, upCommand.RealCommandName);
            btnScript.SetOnButtonData(func.ItemId, onCommand.RealCommandName);
            btnScript.SetOffButtonData(func.ItemId, offCommand.RealCommandName);

            btnObject.transform.SetParent(layoutGroup, false);
        }

        private GameObject SpawnButton(DeviceFunctionality functionality, DeviceCommand command, Transform parent)
        {
            GameObject buttonPrefab = PrefabHolder.Instance.devices.defaultDeviceButton;
            GameObject buttonInstance = Instantiate(buttonPrefab);
            buttonInstance.transform.SetParent(parent, false);
            DefaultDeviceButtonBehavior behavior = buttonInstance.GetComponent<DefaultDeviceButtonBehavior>();
            behavior.CommandDisplayName = command.Name;
            behavior.RealCommandName = command.RealCommandName;
            behavior.DeviceId = functionality.ItemId;
            return buttonInstance;
        }

        private List<DeviceFunctionality> GetOffUpDownFunctionalities(DeviceInfo info)
        {
            if (info.Functionalities == null) return new List<DeviceFunctionality>();

            return
               (from func in info.Functionalities
                where func.Commands.Any(c => TYPE_ON_COMMAND == c.CommandType)
                && func.Commands.Any(c => TYPE_OFF_COMMAND == c.CommandType)
                && func.Commands.Any(c => TYPE_DOWN_COMMAND == c.CommandType)
                && func.Commands.Any(c => TYPE_UP_COMMAND == c.CommandType)
                select func).ToList();
        }

        private const string TYPE_ON_COMMAND = "dogont:OnCommand";
        private const string TYPE_OFF_COMMAND = "dogont:OffCommand";
        private const string TYPE_DOWN_COMMAND = "dogont:DownCommand";
        private const string TYPE_UP_COMMAND = "dogont:UpCommand";


    }
}
