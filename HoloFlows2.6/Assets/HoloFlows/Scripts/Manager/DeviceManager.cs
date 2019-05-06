
using HoloFlows.Client;
using HoloFlows.Model;
using HoloToolkit.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloFlows.Manager
{
    public class DeviceManager : Singleton<DeviceManager>
    {
        /// <summary>
        /// Contains the cached device information. Use <see cref="GetDeviceInfo(string, Action{DeviceInfo})"/> for getting the latest device information.
        /// </summary>
        public Dictionary<string, DeviceInfo> DeviceInfos { get; private set; } = new Dictionary<string, DeviceInfo>();

        /// <summary>
        /// Constains the current state for each device
        /// </summary>
        public Dictionary<string, string> ItemStates { get; private set; } = new Dictionary<string, string>();

        //polling every seconds
        private const int POLLING_DELAY = 2;
        private const int INITIAL_POLLING_DELAY = 5;
        private bool stopPolling = false;

        //TODO Get this uri somehow via the discovery service
        private string openhabUri = "http://192.168.1.115:8080/";

        //TODO device state formatter

        void Start()
        {
            StartCoroutine(RequestAllThings());
            StartCoroutine(PollOpenHab(INITIAL_POLLING_DELAY));
        }

        /// <summary>
        /// Gets the current item value. Returns null if no value was found.
        /// </summary>
        public string GetItemState(string itemName)
        {
            string outValue = null;
            if (!ItemStates.TryGetValue(itemName, out outValue))
            {
                Debug.LogWarningFormat("No value found for item '{0}'", itemName);
            }
            return outValue;
        }

        /// <summary>
        /// Gets the device informations for the given thing id.
        /// First the cached devices data is queried. If no device info for the given uid was found, a web request is executed to get the latest device data.
        /// </summary>
        /// <param name="thingId">thing id</param>
        /// <param name="handleDeviceInfo">the callback method</param>
        public void GetDeviceInfo(string thingId, Action<DeviceInfo> handleDeviceInfo)
        {
            DeviceInfo outValue = null;
            if (!DeviceInfos.TryGetValue(thingId, out outValue))
            {
                StartCoroutine(RequestAllThings(thingId, handleDeviceInfo));
            }
            else
            {
                handleDeviceInfo(outValue);
            }
        }

        private IEnumerator PollOpenHab(int delay)
        {
            var request = new AllItemsShortGetRequest(openhabUri, HandlePollingData);
            yield return request.ExecuteRequest();
            yield return new WaitForSeconds(delay);

            if (!stopPolling) { StartCoroutine(PollOpenHab(POLLING_DELAY)); }
        }

        /// <summary>
        /// Requests all things data from th SAL. If thingUid and callback action added, the callback is called with the device data.
        /// </summary>
        private IEnumerator RequestAllThings(string thingUid = null, Action<DeviceInfo> handleDeviceInfo = null)
        {
            var request = new AllThingsRequest(openhabUri, HandleAllThingsData);
            yield return request.ExecuteRequest();
            if (thingUid != null)
            {
                DeviceInfo outInfo = null;
                if (!DeviceInfos.TryGetValue(thingUid, out outInfo))
                {
                    Debug.LogErrorFormat("device info not found for uid '{0}'", thingUid);
                }
                handleDeviceInfo?.Invoke(outInfo);
            }
        }

        private void HandlePollingData(List<ItemDataShort> itemData)
        {
            foreach (var item in itemData)
            {
                if (ItemStates.ContainsKey(item.name))
                {
                    ItemStates[item.name] = item.state;
                }
                else
                {
                    ItemStates.Add(item.name, item.state);
                }
            }
        }

        private void HandleAllThingsData(List<DeviceInfo> deviceInfos)
        {
            foreach (DeviceInfo info in deviceInfos)
            {
                //TODO updating device information could be useful someday
                //so we could just override all and update the ui?
                if (!DeviceInfos.ContainsKey(info.Uid))
                {
                    DeviceInfos.Add(info.Uid, info);
                }
            }
        }

    }
}

