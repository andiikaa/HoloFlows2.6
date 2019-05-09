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

            foreach (var grouping in groupedFuncs)
            {
                AddButtonsToGroup(grouping);
            }
        }

        private void AddButtonsToGroup(IGrouping<string, DeviceFunctionality> funcs)
        {
            GroupBoxHolder holder = GetHolderById(funcs.Key);
            List<DeviceFunctionality> onOffUpDown = GetOffUpDownFunctionalities(funcs.ToList());

            if (onOffUpDown.Count > 0)
            {
                AddOnOffUpDownCombination(holder.transform, onOffUpDown);
            }

            DeviceFunctionality colorFunc = funcs.Where(f => FUNC_TYPE_COLOR_CONTROL == f.FunctionalityType).FirstOrDefault();
            if (colorFunc != null)
            {
                //add color func button
            }

            //foreach (var func in funcs)
            //{
            //    if (onOffUpDown.Contains(func)) continue;
            //    if (func.Commands == null) continue;

            //    foreach (var cmd in func.Commands)
            //    {
            //        SpawnButton(func, cmd, holder.transform);
            //    }
            //}
        }

        private List<DeviceFunctionality> GetOffUpDownFunctionalities(List<DeviceFunctionality> funcs)
        {
            return (from func in funcs
                    where (func.Commands.Any(c => TYPE_ON_COMMAND == c.CommandType)
                    && func.Commands.Any(c => TYPE_OFF_COMMAND == c.CommandType))
                    || (func.Commands.Any(c => TYPE_DOWN_COMMAND == c.CommandType)
                    && func.Commands.Any(c => TYPE_UP_COMMAND == c.CommandType))
                    select func).ToList();
        }

        private GroupBoxHolder GetHolderById(string id)
        {
            if (id == leftHolder.groupBoxId) return leftHolder;
            if (id == rightHolder.groupBoxId) return rightHolder;
            return null; //should not happen
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

        public override DeviceType GetDeviceType() { return DeviceType.TWO_PIECE; }


    }
}
