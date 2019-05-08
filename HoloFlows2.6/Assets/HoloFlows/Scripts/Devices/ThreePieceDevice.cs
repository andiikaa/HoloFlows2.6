using HoloFlows.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HoloFlows.Devices
{
    public class ThreePieceDevice : DeviceBehaviorBase
    {
        public List<DeviceInfo> DeviceInfo { get; private set; } = new List<DeviceInfo>();

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
            //TODO implement update
            Debug.LogFormat("implement update for {0}", gameObject.name);
        }

        public void SetDeviceInfos(DeviceInfo info)
        {
            //TODO what if we have one state and 3 buttons?
            if (info.GroupBoxes.Count() != 3 || info.States.Count() < 3)
            {
                Debug.LogError("ThreePieceDevice can only handle 3 different states!");
                return;
            }

            SetDeviceState(gameObject.transform.Find("RightDevice/RightGroup"), info.States[0]);
            SetDeviceState(gameObject.transform.Find("MiddleDevice/MiddleGroup"), info.States[1]);
            SetDeviceState(gameObject.transform.Find("LeftDevice/LeftGroup"), info.States[2]);
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
