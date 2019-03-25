using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static DeviceManager;

public class TwoWayDevice : DeviceBehaviorBase
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
        CopyGroup(basic1.transform.Find("EmptyBillboardgroup"), transform.Find("RightDevice/RightGroup"));
        CopyGroup(basic2.transform.Find("EmptyBillboardgroup"), transform.Find("LeftDevice/LeftGroup"));
    }

    private void CopyGroup(Transform groupSource, Transform groupTarget)
    {
        Transform sourceCanvas = groupSource.Find("Canvas");
        Transform soruceImage = sourceCanvas.Find("Image");
        var sDesc = sourceCanvas.Find("Description").GetComponent<Text>();
        var sValue = sourceCanvas.Find("Value").GetComponent<Text>();

        Transform targetCanvas = groupTarget.Find("Canvas");
        targetCanvas.Find("Image");
        var tDesc = targetCanvas.Find("Description").GetComponent<Text>();
        var tValue = targetCanvas.Find("Value").GetComponent<Text>();

        tDesc.text = sDesc.text;
        tValue.text = sValue.text;

        if (groupSource.childCount > 1)
        {
            for (int i = 1; i < groupSource.childCount; i++)
            {
                Transform child = groupSource.GetChild(i);
                CreateNewButtons(groupTarget, child);
            }
        }
    }

    private void CreateNewButtons(Transform parent, Transform sourceChild)
    {
        GameObject newChild = Instantiate(sourceChild.gameObject);
        //TODO check if button!

        DefaultDeviceButtonBehavior[] sourceBehaviors = sourceChild.GetComponentsInChildren<DefaultDeviceButtonBehavior>();
        DefaultDeviceButtonBehavior[] targetBehaviors = newChild.GetComponentsInChildren<DefaultDeviceButtonBehavior>();

        if (sourceBehaviors.Length != targetBehaviors.Length)
        {
            Debug.LogError("defaultbuttonbehaviors count does not match for copied instance");
            return;
        }

        for (int i = 0; i < sourceBehaviors.Length; i++)
        {
            targetBehaviors[i].DeviceId = sourceBehaviors[i].DeviceId;
            targetBehaviors[i].RealCommandName = sourceBehaviors[i].RealCommandName;
            targetBehaviors[i].CommandDisplayName = sourceBehaviors[i].CommandDisplayName;
        }

        newChild.transform.SetParent(parent, false);
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
