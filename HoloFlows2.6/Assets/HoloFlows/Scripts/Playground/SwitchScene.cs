using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class SwitchScene : MonoBehaviour, IInputClickHandler, IFocusable
{

    public string SceneName = "TODO";


    public void OnFocusEnter()
    {
    }

    public void OnFocusExit()
    {
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        SceneOrganizer.Instance.SwitchToScene(SceneName);
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
