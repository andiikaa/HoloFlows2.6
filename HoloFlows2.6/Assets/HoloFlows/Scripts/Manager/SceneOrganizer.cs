using HoloToolkit.Unity;
using UnityEngine.SceneManagement;

public class SceneOrganizer : Singleton<SceneOrganizer>
{

    private const string MAIN_SCENE = "main";
    private const string CONTROL_SCENE = "control";
    private const string LEARN_SCENE = "learn";



    //https://forums.hololens.com/discussion/7726/how-do-i-change-a-scene-on-air-tapping-on-an-object

    //https://docs.unity3d.com/ScriptReference/SceneManagement.LoadSceneMode.Additive.html

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchToMain() { }

    public void SwitchToScene(string name)
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(CONTROL_SCENE, LoadSceneMode.Additive);
    }
}
