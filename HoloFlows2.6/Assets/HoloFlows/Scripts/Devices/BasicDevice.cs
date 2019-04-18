
namespace HoloFlows.Devices
{
    public class BasicDevice : DeviceBehaviorBase
    {
        protected override DeviceType GetDeviceType() { return DeviceType.BASIC; }

        public void Start()
        {

        }

        //TODO implement and test this stuff for all devices
        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        public override void Show()
        {
            gameObject.SetActive(true);
        }
    }
}

