using HoloFlows.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HoloFlows.Devices
{
    public class TwoPieceDevice : DeviceBehaviorBase
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
            if (info.GroupBoxes.Count() != 2 || info.States.Count() < 2)
            {
                Debug.LogError("TwoPieceDevice can only handle 2 different states!");
                return;
            }

            //find all states

            SetDeviceState(gameObject.transform.Find("RightDevice/RightGroup"), info.States[0]);
            SetDeviceState(gameObject.transform.Find("LeftDevice/LeftGroup"), info.States[1]);
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
