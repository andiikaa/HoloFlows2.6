using HoloFlows.ButtonScripts;
using HoloFlows.Manager;
using HoloFlows.ObjectDetection;
using HoloToolkit.Unity.InputModule;

public class SpawnDevice : TapSoundButton
{
    public override void HandleClickEvent(InputClickedEventData eventData)
    {
        //HoloFlowSceneManager.Instance.SwitchToQRScan();
        //DeviceSpawner.Instance.SpawnDevice("hue_bulb210_1", e => { });
        //HoloFlowSceneManager.Instance.SwitchToEdit();
        HoloFlowSceneManager.Instance.SwitchToQRScan();
        HoloFlowSceneManager.Instance.SwitchToWizard(QRCodeData.FromQrCodeData(""));
    }

}
