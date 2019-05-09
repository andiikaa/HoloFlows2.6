using HoloFlows.Manager;
using HoloFlows.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HoloFlows.Devices
{
    public class BasicDevice : DeviceBehaviorBase
    {
        public override DeviceType GetDeviceType() { return DeviceType.BASIC; }

        public DeviceInfo DeviceInfo { get; private set; }

        private Text description;
        private Text stateValue;

        void Start()
        {
            RegisterToHoloFlowSceneManager();
        }

        void OnDestroy()
        {
            UnregisterFromHoloFlowSceneManager();
        }

        private void Update()
        {
            UpdateDeviceStatesInternal();
        }

        public void SetDeviceInfo(DeviceInfo info)
        {
            DeviceInfo = info;
            UpdateDeviceStates();
            AddButtons();
        }

        protected override void UpdateDeviceStates()
        {
            if (DeviceInfo == null || DeviceInfo.States == null || !DeviceInfo.States.Any()) { return; }

            if (DeviceInfo.States.Count() > 1) { Debug.LogWarningFormat("{0} contains more than one state. Only the first is shown", DeviceInfo.Uid); }

            DeviceState deviceState = DeviceInfo.States[0];
            if (DeviceManager.IsInitialized)
            {
                string realStateValue = DeviceManager.Instance.GetItemState(deviceState.ItemId);
                if (!string.IsNullOrEmpty(realStateValue)) { deviceState.RealStateValue = realStateValue; }
                SetDeviceState(gameObject.transform.Find("EmptyBillboardgroup"), deviceState);
            }
        }

        private void AddButtons()
        {
            if (DeviceInfo.Functionalities == null || !DeviceInfo.Functionalities.Any())
            {
                Debug.LogWarning("no functionalities for basic device");
                return;
            }
            Transform btnLayoutGroup = gameObject.transform.Find("EmptyBillboardgroup");
            if (!AddButtonsToLayoutGroup(btnLayoutGroup, DeviceInfo))
            {
                Debug.LogError("VerticalLayoutGroup not found for basic device");
            }
        }

        #region Gen Buttons

        protected bool AddButtonsToLayoutGroup(Transform layoutGroup, DeviceInfo info)
        {
            if (layoutGroup == null || layoutGroup.GetComponent<VerticalLayoutGroup>() == null)
                return false;

            List<DeviceFunctionality> onOffUpDownFunc = GetOffUpDownFunctionalities(info);
            if (onOffUpDownFunc.Any())
            {
                AddOnOffUpDownCombination(layoutGroup, onOffUpDownFunc);
            }

            foreach (var func in info.Functionalities)
            {
                if (onOffUpDownFunc.Contains(func)) continue;
                if (func.Commands == null) continue;

                foreach (var cmd in func.Commands)
                {
                    SpawnButton(func, cmd, layoutGroup);
                }
            }

            return true;
        }

        /// <summary>
        /// Gets functionalities which supports on, off, up and down
        /// </summary>
        private List<DeviceFunctionality> GetOffUpDownFunctionalities(DeviceInfo info)
        {
            if (info.Functionalities == null) return new List<DeviceFunctionality>();

            var groupedFunc = GetGroupedFunctionalities(info.Functionalities.ToList());
            if (groupedFunc == null) { return new List<DeviceFunctionality>(); }

            //basic device - only one group
            var firstGroup = groupedFunc.FirstOrDefault();
            if (firstGroup == null) { return new List<DeviceFunctionality>(); }

            return (from func in firstGroup
                    where (func.Commands.Any(c => TYPE_ON_COMMAND == c.CommandType)
                    && func.Commands.Any(c => TYPE_OFF_COMMAND == c.CommandType))
                    || (func.Commands.Any(c => TYPE_DOWN_COMMAND == c.CommandType)
                    && func.Commands.Any(c => TYPE_UP_COMMAND == c.CommandType))
                    select func).ToList();
        }

        #endregion

        private void SetDeviceState(Transform transform, DeviceState deviceState)
        {
            //TODO Image
            //transform.Find("Canvas/Image");
            if (description == null)
            {
                description = transform.Find("Canvas/Description").GetComponent<Text>();
                description.text = deviceState.GroupBox.Name;
            }

            if (stateValue == null) { stateValue = transform.Find("Canvas/Value").GetComponent<Text>(); }
            string realStateValue = deviceState.RealStateValue ?? Settings.UNKNOWN_STATE;
            stateValue.text = deviceState.RealStateValue + " " + GetValuePrefix(deviceState.UnitOfMeasure);
        }


    }
}

