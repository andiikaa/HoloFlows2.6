using HoloFlows.ButtonScripts;
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
        protected override DeviceType GetDeviceType() { return DeviceType.BASIC; }

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
            AddContent();
            UpdateDeviceStates();
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

        private void AddContent()
        {
            Transform btnLayoutGroup = gameObject.transform.Find("EmptyBillboardgroup");
            if (!AddButtonsToLayoutGroup(btnLayoutGroup, DeviceInfo))
            {
                Debug.LogError("failed to find the layout group for the buttons");
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
            return true;
        }

        private void AddOnOffUpDownCombination(Transform layoutGroup, List<DeviceFunctionality> onOffUpDownFunc)
        {
            DeviceFunctionality onCommandFunc = onOffUpDownFunc
                .Where(f => f.Commands.Any(c => TYPE_ON_COMMAND == c.CommandType))
                .First();
            DeviceFunctionality offCommandFunc = onOffUpDownFunc
                .Where(f => f.Commands.Any(c => TYPE_OFF_COMMAND == c.CommandType))
                .First();
            DeviceFunctionality upCommandFunc = onOffUpDownFunc
                .Where(f => f.Commands.Any(c => TYPE_UP_COMMAND == c.CommandType))
                .First();
            DeviceFunctionality downCommandFunc = onOffUpDownFunc
                .Where(f => f.Commands.Any(c => TYPE_DOWN_COMMAND == c.CommandType))
                .First();

            //buttons are visible in this order from top to bottom
            GameObject btnObject = Instantiate(PrefabHolder.Instance.devices.onOffUpDownButton);
            OnOffUpDownButton btnScript = btnObject.GetComponent<OnOffUpDownButton>();

            btnScript.SetDownButtonData(downCommandFunc.ItemId, downCommandFunc.Commands.First(c => TYPE_DOWN_COMMAND == c.CommandType).RealCommandName);
            btnScript.SetUpButtonData(upCommandFunc.ItemId, upCommandFunc.Commands.First(c => TYPE_UP_COMMAND == c.CommandType).RealCommandName);
            btnScript.SetOnButtonData(onCommandFunc.ItemId, onCommandFunc.Commands.First(c => TYPE_ON_COMMAND == c.CommandType).RealCommandName);
            btnScript.SetOffButtonData(offCommandFunc.ItemId, offCommandFunc.Commands.First(c => TYPE_OFF_COMMAND == c.CommandType).RealCommandName);

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

