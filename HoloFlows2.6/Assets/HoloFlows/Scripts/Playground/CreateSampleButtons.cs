using UnityEngine;

public class CreateSampleButtons : MonoBehaviour
{

    private Transform group;

    private const float OFFSET = 0.1f;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Creating sample buttons");

        group = gameObject.transform.Find("EmptyBillboardgroup");
        GameObject defaultButton = PrefabHolder.Instance.defaultDeviceButton;
        //Instatiate makes copy of the object
        GameObject buttonInstance = Instantiate(defaultButton);
        buttonInstance.transform.SetParent(group, false);
        DefaultDeviceButtonBehavior behavior1 = buttonInstance.GetComponent<DefaultDeviceButtonBehavior>();
        behavior1.CommandDisplayName = "YUHU";
        behavior1.RealCommandName = "FTW";
        behavior1.DeviceId = "Super_Cool_Item";

        GameObject buttonInstance2 = Instantiate(defaultButton);
        buttonInstance2.transform.SetParent(group, false);
        buttonInstance2.GetComponent<DefaultDeviceButtonBehavior>();
        DefaultDeviceButtonBehavior behavior2 = buttonInstance2.GetComponent<DefaultDeviceButtonBehavior>();
        behavior2.CommandDisplayName = "YUHU2";
        behavior2.RealCommandName = "FTW_FTW";
        behavior2.DeviceId = "Some_Other_Cool_Item";

        float newX = buttonInstance2.transform.localPosition.x;
        float newY = buttonInstance.transform.localPosition.y + OFFSET;
        float newZ = buttonInstance2.transform.localPosition.z;

        buttonInstance2.transform.localPosition.Set(newX, newY, newZ);

        //buttonInstance2.transform.localPosition.Set()

        //buttonInstance.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
