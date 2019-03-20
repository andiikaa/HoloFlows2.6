using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class SwitchScene : MonoBehaviour, IInputClickHandler, IFocusable
{

    public void OnFocusEnter()
    {
    }

    public void OnFocusExit()
    {
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("Wizard should open");
        Instantiate(PrefabHolder.Instance.assemblyWizard).SetActive(true);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
