using HoloFlows;
using HoloFlows.ButtonScripts;
using System.Collections;
using UnityEngine;

public class CreateSampleButtons : MonoBehaviour
{

    private Transform group;
    private int nextObjectIndex = 0;

    private const float OFFSET = 0.1f;
    private GameObject defaultButton;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Creating sample buttons");

        group = gameObject.transform.Find("EmptyBillboardgroup");
        nextObjectIndex = group.childCount;

        defaultButton = PrefabHolder.Instance.devices.defaultDeviceButton;
        //Instatiate makes copy of the object
        GameObject buttonInstance = Instantiate(defaultButton);
        buttonInstance.transform.SetParent(group, false);
        //buttonInstance.transform.SetSiblingIndex(0);
        DefaultDeviceButtonBehavior behavior1 = buttonInstance.GetComponent<DefaultDeviceButtonBehavior>();
        behavior1.CommandDisplayName = "YUHU";
        behavior1.RealCommandName = "FTW";
        behavior1.DeviceId = "Super_Cool_Item";

        GameObject buttonInstance2 = Instantiate(defaultButton);
        buttonInstance2.transform.SetParent(group, false);
        //buttonInstance2.transform.SetSiblingIndex(0);
        buttonInstance2.GetComponent<DefaultDeviceButtonBehavior>();
        DefaultDeviceButtonBehavior behavior2 = buttonInstance2.GetComponent<DefaultDeviceButtonBehavior>();
        behavior2.CommandDisplayName = "YUHU2";
        behavior2.RealCommandName = "FTW_FTW";
        behavior2.DeviceId = "Some_Other_Cool_Item";

        StartCoroutine(SpawnButtons());

        //float newX = buttonInstance2.transform.localPosition.x;
        //float newY = buttonInstance.transform.localPosition.y + OFFSET;
        //float newZ = buttonInstance2.transform.localPosition.z;

        //buttonInstance2.transform.localPosition.Set(newX, newY, newZ);

        //buttonInstance2.transform.localPosition.Set()

        //buttonInstance.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator SpawnButtons()
    {
        for (int i = 0; i < 10; i++)
        {
            SpawnSimpleButton("", i);
            yield return new WaitForSeconds(1);
        }

        yield break;
    }

    private void SpawnSimpleButton(string text, int id)
    {
        GameObject buttonInstance = Instantiate(defaultButton);
        buttonInstance.transform.SetParent(group, false);
        //buttonInstance2.transform.SetSiblingIndex(0);
        buttonInstance.GetComponent<DefaultDeviceButtonBehavior>();
        DefaultDeviceButtonBehavior behavior = buttonInstance.GetComponent<DefaultDeviceButtonBehavior>();
        behavior.CommandDisplayName = "Spawned" + id;
        behavior.RealCommandName = "FTW_FTW_Spawned" + id;
        behavior.DeviceId = "Some_Other_Spawned_Item_" + id;
    }
}
