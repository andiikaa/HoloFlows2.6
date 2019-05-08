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

        private GroupBoxHolder leftHolder;
        private GroupBoxHolder rightHolder;

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

        private void UpdateStateAndSetValues(GroupBoxHolder holder)
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
            InitGroupBoxes();
            if (leftHolder == null || rightHolder == null)
            {
                Debug.LogError("Could not init device information. Failed to set all groupboxes");
                return;
            }
            AddStates();
            UpdateDeviceStates();
            AddButtons();
        }

        private void InitGroupBoxes()
        {
            List<GroupBox> boxes = new List<GroupBox>();
            foreach (var info in DeviceInfos) { boxes.AddRange(info.GroupBoxes); }

            if (boxes.Count != 2)
            {
                Debug.LogError("TwoPieceDevice needs 2 GroupBoxes!");
                return;
            }

            leftHolder = new GroupBoxHolder()
            {
                groupBoxId = boxes[0].Uid,
                transform = gameObject.transform.Find("LeftDevice/LeftGroup")
            };
            rightHolder = new GroupBoxHolder()
            {
                groupBoxId = boxes[1].Uid,
                transform = gameObject.transform.Find("RightDevice/RightGroup")
            };
        }

        private void AddStates()
        {
            List<DeviceState> states = new List<DeviceState>();
            foreach (var tmpInfo in DeviceInfos)
            {
                if (tmpInfo.States != null) states.AddRange(tmpInfo.States);
            }

            var groupedStates = GetGroupedStates(states);
            if (groupedStates == null || groupedStates.Count() == 0)
            {
                Debug.LogWarning("TwoPieceDevice with no states");
                return;
            }

            var groupedList = groupedStates.ToList();

            DeviceState leftState = groupedList.FirstOrDefault(e => e.Key == leftHolder.groupBoxId)?.FirstOrDefault();
            DeviceState rightState = groupedList.FirstOrDefault(e => e.Key == rightHolder.groupBoxId)?.FirstOrDefault();

            leftHolder.deviceState = leftState;
            rightHolder.deviceState = rightState;

            //if (groupedList.Count > 0)
            //{
            //    DeviceState firstState = groupedList[0].First();
            //    rightHolder = new GroupBoxHolder()
            //    {
            //        transform = gameObject.transform.Find("RightDevice/RightGroup"),
            //        deviceState = firstState
            //    };
            //}

            //if (groupedList.Count > 1)
            //{
            //    DeviceState secondState = groupedList[1].First();
            //    leftHolder = new GroupBoxHolder()
            //    {
            //        transform = gameObject.transform.Find("LeftDevice/LeftGroup"),
            //        deviceState = secondState
            //    };
            //}
        }

        private void AddButtons()
        {
            List<DeviceFunctionality> funcs = new List<DeviceFunctionality>();
            foreach (var tmpInfo in DeviceInfos)
            {
                if (tmpInfo.Functionalities != null) funcs.AddRange(tmpInfo.Functionalities);
            }

            var groupedFuncs = GetGroupedFunctionalities(funcs);
            if (groupedFuncs == null || groupedFuncs.Count() == 0)
            {
                Debug.LogWarning("TwoPieceDevice with no functions");
                return;
            }

            var groupedList = groupedFuncs.ToList();
            //groupedList.Where(k => k.Key == leftHolder.)

            //TODO Buttons

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
