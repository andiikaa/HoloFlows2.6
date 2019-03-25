using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static DeviceManager;

public class ThreeWayDevice : DeviceBehaviorBase
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetDeviceInfos(DeviceInfo info)
    {
        IEnumerable<string> stateItems = GetStateItems(info);
        if (stateItems.Count() != 3)
        {
            Debug.LogError("TwoWayDevice can only handle 2 different states!");
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

    private static string GetValuePrefix(UnitOfMeasure unitOfMeasure)
    {
        if (unitOfMeasure == null || string.IsNullOrEmpty(unitOfMeasure.PrefixSymbol))
        {
            return string.Empty;
        }
        return unitOfMeasure.PrefixSymbol;
    }

    private IEnumerable<string> GetStateItems(DeviceInfo info)
    {
        return info.States
            .Select(e => e.ItemId)
            .GroupBy(e => e).Select(e => e.Key)
            .Distinct();
    }

    protected override DeviceType GetDeviceType() { return DeviceType.THREE_WAY; }
}
