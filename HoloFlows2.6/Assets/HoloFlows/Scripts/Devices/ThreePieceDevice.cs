using HoloFlows.Manager;
using HoloFlows.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HoloFlows.Devices
{
    public class ThreePieceDevice : DeviceBehaviorBase
    {
        public List<DeviceInfo> DeviceInfos { get; private set; } = new List<DeviceInfo>();

        private DeviceStateHolder leftHolder;
        private DeviceStateHolder middleHolder;
        private DeviceStateHolder rightHolder;

        void Start()
        {
            RegisterToHoloFlowSceneManager();
        }

        void Update()
        {
            UpdateDeviceStatesInternal();
        }

        void OnDestroy()
        {
            UnregisterFromHoloFlowSceneManager();
        }

        protected override void UpdateDeviceStates()
        {
            UpdateStateAndSetValues(rightHolder);
            UpdateStateAndSetValues(middleHolder);
            UpdateStateAndSetValues(leftHolder);
        }

        private void UpdateStateAndSetValues(DeviceStateHolder holder)
        {
            if (holder != null)
            {
                if (DeviceManager.IsInitialized)
                {
                    string realStateValue = DeviceManager.Instance.GetItemState(holder.deviceState.ItemId);
                    if (realStateValue != null) holder.deviceState.RealStateValue = realStateValue;
                }
                SetDeviceState(holder.transform, holder.deviceState);
            }
        }

        public void SetDeviceInfos(params DeviceInfo[] infos)
        {
            DeviceInfos.AddRange(infos.ToList());

            List<DeviceState> states = new List<DeviceState>();
            foreach (var tmpInfo in DeviceInfos)
            {
                states.AddRange(tmpInfo.States);
            }

            var groupedStates = GetGroupedStates(states);
            if (groupedStates == null || groupedStates.Count() == 0)
            {
                Debug.LogWarning("ThreePieceDevice with no states");
                return;
            }

            var groupedList = groupedStates.ToList();

            if (groupedList.Count > 0)
            {
                DeviceState firstState = groupedList[0].First();
                rightHolder = new DeviceStateHolder()
                {
                    transform = gameObject.transform.Find("RightDevice/RightGroup"),
                    deviceState = firstState
                };
            }

            if (groupedList.Count > 1)
            {
                DeviceState secondState = groupedList[1].First();
                middleHolder = new DeviceStateHolder()
                {
                    transform = gameObject.transform.Find("MiddleDevice/MiddleGroup"),
                    deviceState = secondState
                };
            }

            if (groupedList.Count > 2)
            {
                DeviceState thirdState = groupedList[2].First();
                leftHolder = new DeviceStateHolder()
                {
                    transform = gameObject.transform.Find("LeftDevice/LeftGroup"),
                    deviceState = thirdState
                };
            }

            UpdateDeviceStates();
        }

        private void SetDeviceState(Transform transform, DeviceState deviceState)
        {
            //TODO Image
            //transform.Find("Canvas/Image");
            Text description = transform.Find("Canvas/Description").GetComponent<Text>();
            Text value = transform.Find("Canvas/Value").GetComponent<Text>();

            description.text = GetLabelOrItemId(deviceState);
            value.text = deviceState.RealStateValue + " " + GetValuePrefix(deviceState.UnitOfMeasure);
        }

        private string GetLabelOrItemId(DeviceState state)
        {
            return string.IsNullOrEmpty(state.Label) ? state.ItemId : state.Label;
        }

        protected override DeviceType GetDeviceType() { return DeviceType.THREE_PIECE; }

    }
}
