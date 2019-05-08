using HoloFlows.ButtonScripts;
using HoloFlows.Devices;
using HoloFlows.Manager;
using HoloToolkit.Unity.InputModule;

public class SpawnDevice : TapSoundButton
{
    public override void HandleClickEvent(InputClickedEventData eventData)
    {
        //HoloFlowSceneManager.Instance.SwitchToQRScan();
        DeviceSpawner.Instance.SpawnDevice("hue_bulb210_1", e => { });
        HoloFlowSceneManager.Instance.SwitchToEdit();
    }

}
