using HoloFlows.Manager;
using HoloFlows.Model;
using HoloToolkit.Unity;
using System;
using System.Linq;
using UnityEngine;

namespace HoloFlows.Devices
{
    public class DeviceSpawner : Singleton<DeviceSpawner>
    {
        private DeviceManager deviceManager;

        // Use this for initialization
        void Start()
        {

        }

        /// <summary>
        /// Spawn a 
        /// </summary>
        /// <param name="deviceUid"></param>
        /// <returns></returns>
        public void SpawnDevice(string deviceUid, Action<GameObject> handleGameObject)
        {
            //TODO use the deviceUid from the scan
            //deviceUid = "hue_bulb210_1";
            deviceUid = "hue_bulb210_1";

            //Get The DeviceManager for a bit shorter syntax
            if (deviceManager == null)
            {
                deviceManager = DeviceManager.Instance;
            }

            deviceManager.GetDeviceInfo(deviceUid, info =>
            {
                if (info == null)
                {
                    Debug.LogErrorFormat("device info for '{0}' not found", deviceUid);
                    handleGameObject(null);
                }
                else
                {
                    GameObject go = GetDeviceSpecificPrefab(info);
                    if (go == null)
                    {
                        Debug.LogErrorFormat("failed to spawn device for uid '{0}'", deviceUid);
                    }
                    handleGameObject(go);
                }

            });
        }

        /// <summary>
        /// Merge <see cref="BasicDevice"/> devices to a <see cref="TwoPieceDevice"/>
        /// </summary>
        /// <param name="device1"></param>
        /// <param name="device2"></param>
        /// <returns></returns>
        public GameObject MergeBasicDevices(GameObject device1, GameObject device2)
        {
            if (!device1.name.StartsWith("BasicDevice") || !device2.name.StartsWith("BasicDevice"))
            {
                Debug.LogError("At least one of the merging devices is not from type BasicDevice");
                return null;
            }

            GameObject go = Instantiate(PrefabHolder.Instance.devices.twoPieceDevice);
            TwoPieceDevice mergedDevice = go.GetComponent<TwoPieceDevice>();
            mergedDevice.CopyFromBasicDevices(device1, device2);
            go.SetActive(true);
            return go;
        }

        //get the prefab specific for the device (Basic, 2piece, 3piece, multi)
        private GameObject GetDeviceSpecificPrefab(DeviceInfo info)
        {
            if (info.GroupBoxes == null || !info.GroupBoxes.Any())
            {
                Debug.LogErrorFormat("Device info does not contain any group box: thing: '{0}'", info.Uid);
                return null;
            }

            int differentGroups = info.GroupBoxes.Count();

            if (differentGroups == 1)
            {
                return InstantiateBasicDevice(info);
            }
            else if (differentGroups == 2)
            {
                return InstantiateTwoPieceDevice(info);
            }
            else if (differentGroups == 3)
            {
                return InstantiateThreePieceDevice(info);
            }
            else
            {
                Debug.LogError("Multidevice not supported yet!");
                return null;
            }
        }

        private GameObject InstantiateTwoPieceDevice(DeviceInfo info)
        {
            GameObject go = Instantiate(PrefabHolder.Instance.devices.twoPieceDevice);
            TwoPieceDevice device = go.GetComponent<TwoPieceDevice>();
            device.DeviceId = info.Uid;
            device.SetDeviceInfos(info);
            ActivateGameObjectIfNeeded(go);
            return go;
        }


        private GameObject InstantiateThreePieceDevice(DeviceInfo info)
        {
            GameObject go = Instantiate(PrefabHolder.Instance.devices.threePieceDevice);
            ThreePieceDevice device = go.GetComponent<ThreePieceDevice>();
            device.DeviceId = info.Uid;
            device.SetDeviceInfos(info);
            ActivateGameObjectIfNeeded(go);
            return go;
        }

        private GameObject InstantiateBasicDevice(DeviceInfo info)
        {
            GameObject go = Instantiate(PrefabHolder.Instance.devices.basicDevice);
            BasicDevice device = go.GetComponent<BasicDevice>();
            device.SetDeviceInfo(info);
            device.DeviceId = info.Uid;
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




    }
}
