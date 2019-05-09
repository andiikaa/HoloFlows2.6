using HoloFlows.Manager;
using HoloFlows.Model;
using HoloToolkit.Unity;
using System;
using System.Collections.Generic;
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

        string lastSpawned = null;

        /// <summary>
        /// Spawn a 
        /// </summary>
        /// <param name="deviceUid"></param>
        /// <returns></returns>
        public void SpawnDevice(string deviceUid, Action<GameObject> handleGameObject)
        {
            //TODO use the deviceUid from the scan
            deviceUid = "tinkerforge_irTemp_1";

            if (lastSpawned == null)
            {
                deviceUid = "tinkerforge_irTemp_1";
            }
            else if (lastSpawned == "tinkerforge_irTemp_1")
            {
                deviceUid = "hue_bulb210_1";
            }
            else if (lastSpawned == "hue_bulb210_1")
            {
                deviceUid = "tinkerforge_ambientLight_ambientLight_2";
            }
            else if (lastSpawned == "tinkerforge_ambientLight_ambientLight_2")
            {
                deviceUid = "tinkerforge_irTemp_1";
            }

            lastSpawned = deviceUid;


            //deviceUid = "hue_bulb210_1";
            //deviceUid = "tinkerforge_ambientLight_ambientLight_2";

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
        /// <returns>null if failed</returns>
        public GameObject MergeBasicDevices(BasicDevice left, BasicDevice right)
        {
            return InstantiateTwoPieceDevice(left.DeviceInfo, right.DeviceInfo);
        }

        /// <summary>
        /// Merge a <see cref="BasicDevice"/> and <see cref="TwoPieceDevice"/> to a <see cref="ThreePieceDevice"/>
        /// </summary>
        /// <returns>null if failed</returns>
        public GameObject MergeBasicAndTwoPiece(BasicDevice basic, TwoPieceDevice twoPiece)
        {
            List<DeviceInfo> infos = new List<DeviceInfo>();
            infos.Add(basic.DeviceInfo);
            infos.AddRange(twoPiece.DeviceInfos);
            return InstantiateThreePieceDevice(infos.ToArray());
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

        private GameObject InstantiateTwoPieceDevice(params DeviceInfo[] infos)
        {
            GameObject go = Instantiate(PrefabHolder.Instance.devices.twoPieceDevice);
            TwoPieceDevice device = go.GetComponent<TwoPieceDevice>();
            device.DeviceId = string.Join("__", infos.Select(info => info.Uid));
            device.SetDeviceInfos(infos);
            ActivateGameObjectIfNeeded(go);
            Debug.LogFormat("Spawned TwoPieceDevice with uid '{0}'", device.DeviceId);
            return go;
        }


        private GameObject InstantiateThreePieceDevice(params DeviceInfo[] infos)
        {
            GameObject go = Instantiate(PrefabHolder.Instance.devices.threePieceDevice);
            ThreePieceDevice device = go.GetComponent<ThreePieceDevice>();
            device.DeviceId = string.Join("__", infos.Select(info => info.Uid));
            device.SetDeviceInfos(infos);
            ActivateGameObjectIfNeeded(go);
            Debug.LogFormat("Spawned ThreePieceDevice with uid '{0}'", device.DeviceId);
            return go;
        }

        private GameObject InstantiateBasicDevice(DeviceInfo info)
        {
            GameObject go = Instantiate(PrefabHolder.Instance.devices.basicDevice);
            BasicDevice device = go.GetComponent<BasicDevice>();
            device.SetDeviceInfo(info);
            device.DeviceId = info.Uid;
            ActivateGameObjectIfNeeded(go);
            Debug.LogFormat("Spawned BasicDevice with uid '{0}'", device.DeviceId);
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
