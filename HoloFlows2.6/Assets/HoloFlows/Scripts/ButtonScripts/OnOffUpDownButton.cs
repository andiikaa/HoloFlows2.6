using UnityEngine;

namespace HoloFlows.ButtonScripts
{

    public class OnOffUpDownButton : MonoBehaviour
    {
        public void SetOnButtonData(string deviceId, string realCommandName, string commandDisplayName = "ON")
        {
            SetButtonData(transform.Find("TopButtons/OnButton"), deviceId, realCommandName, commandDisplayName);
        }


        public void SetOffButtonData(string deviceId, string realCommandName, string commandDisplayName = "OFF")
        {
            SetButtonData(transform.Find("OffButton"), deviceId, realCommandName, commandDisplayName);
        }

        public void SetUpButtonData(string deviceId, string realCommandName)
        {
            SetButtonData(transform.Find("TopButtons/UpButton"), deviceId, realCommandName);
        }

        public void SetDownButtonData(string deviceId, string realCommandName)
        {
            SetButtonData(transform.Find("TopButtons/DownButton"), deviceId, realCommandName);
        }


        private void SetButtonData(Transform button, string deviceId, string realCommandName, string commandDisplayName = null)
        {
            DefaultDeviceButtonBehavior btnBehavior = button.GetComponent<DefaultDeviceButtonBehavior>();
            btnBehavior.DeviceId = deviceId;
            btnBehavior.RealCommandName = realCommandName;
            btnBehavior.CommandDisplayName = commandDisplayName;
        }
    }
}