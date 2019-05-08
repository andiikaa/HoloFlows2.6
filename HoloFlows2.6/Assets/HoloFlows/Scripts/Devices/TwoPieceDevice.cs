using HoloFlows.Manager;
using HoloFlows.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HoloFlows.Devices
{
    public class TwoPieceDevice : DeviceBehaviorBase
    {

        public List<DeviceInfo> DeviceInfos { get; private set; } = new List<DeviceInfo>();

        private DeviceStateHolder leftHolder;
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
            //TODO MERGE WITH DEVICE INFO!

            DeviceInfos.AddRange(infos.ToList());

            List<DeviceState> states = new List<DeviceState>();
            foreach (var tmpInfo in DeviceInfos)
            {
                states.AddRange(tmpInfo.States);
            }

            var groupedStates = GetGroupedStates(states);
            if (groupedStates == null || groupedStates.Count() == 0)
            {
                Debug.LogWarning("TwoPieceDevice with no states");
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
                leftHolder = new DeviceStateHolder()
                {
                    transform = gameObject.transform.Find("LeftDevice/LeftGroup"),
                    deviceState = secondState
                };
            }

            UpdateDeviceStates();
        }

        public void CopyFromBasicDevices(GameObject basic1, GameObject basic2)
        {
            CopyBillboardGroup(basic1.transform.Find("EmptyBillboardgroup"), transform.Find("RightDevice/RightGroup"));
            CopyBillboardGroup(basic2.transform.Find("EmptyBillboardgroup"), transform.Find("LeftDevice/LeftGroup"));
        }

        private void SetDeviceState(Transform transform, DeviceState deviceState)
        {
            //TODO Image
            //transform.Find("Canvas/Image");
            Text description = transform.Find("Canvas/Description").GetComponent<Text>();
            Text value = transform.Find("Canvas/Value").GetComponent<Text>();

            description.text = deviceState.GroupBox.Name;
            value.text = deviceState.RealStateValue + " " + GetValuePrefix(deviceState.UnitOfMeasure);
        }

        protected override DeviceType GetDeviceType() { return DeviceType.TWO_PIECE; }


    }
}
