
namespace HoloFlows.Devices
{
    public class BasicDevice : DeviceBehaviorBase
    {
        protected override DeviceType GetDeviceType() { return DeviceType.BASIC; }

        void Start()
        {
            RegisterToHoloFlowSceneManager();
        }

        void OnDestroy()
        {
            UnregisterFromHoloFlowSceneManager();
        }

    }
}

