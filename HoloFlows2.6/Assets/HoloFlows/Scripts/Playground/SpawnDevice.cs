using HoloFlows.ButtonScripts;
using HoloFlows.Devices;
using HoloToolkit.Unity.InputModule;

public class SpawnDevice : TapSoundButton
{
    public override void HandleClickEvent(InputClickedEventData eventData)
    {
        DeviceSpawner.Instance.SpawnDevice("Some_device_uid");
    }

}
