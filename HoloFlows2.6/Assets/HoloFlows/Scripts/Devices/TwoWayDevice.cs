using HoloFlows.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HoloFlows.Devices
{
    public class TwoWayDevice : DeviceBehaviorBase
    {

        // Use this for initialization
        void Start()
        {
            RegisterToHoloFlowSceneManager();
        }

        // Update is called once per frame
        void Update()
        {
        }

        void OnDestroy()
        {
            UnregisterFromHoloFlowSceneManager();
        }

        public void SetDeviceInfos(DeviceInfo info)
        {
            IEnumerable<string> stateItems = GetStateItems(info);
            if (stateItems.Count() != 2)
            {
                Debug.LogError("TwoWayDevice can only handle 2 different states!");
                return;
            }

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

            description.text = deviceState.ItemId;
            value.text = deviceState.RealStateValue + " " + GetValuePrefix(deviceState.UnitOfMeasure);
        }

        private IEnumerable<string> GetStateItems(DeviceInfo info)
        {
            return info.States
                .Select(e => e.ItemId)
                .GroupBy(e => e).Select(e => e.Key)
                .Distinct();
        }

        private static string GetValuePrefix(UnitOfMeasure unitOfMeasure)
        {
            if (unitOfMeasure == null || string.IsNullOrEmpty(unitOfMeasure.PrefixSymbol))
            {
                return string.Empty;
            }
            return unitOfMeasure.PrefixSymbol;
        }

        protected override DeviceType GetDeviceType() { return DeviceType.TWO_WAY; }

    }
}
