using HoloToolkit.UX.Progress;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WizardDialog : MonoBehaviour
{
    private const string MAIN_CONTENT = "MainContent";
    private const string NEXT_BTN = "NextButton";
    private const string PROGRESS_INDIC = "ProgressIndicator";
    private const string MAIN_CONTENT_TEXT = "Text";
    private const string MAIN_CONTENT_IMAGE = "Image";


    private GameObject mainContent;
    private GameObject nextBtn;
    private Text mainText;
    private Image mainImage;


    private WizardTask nextTask;

    void Start()
    {
        mainContent = gameObject.transform.Find(MAIN_CONTENT).gameObject;
        nextBtn = gameObject.transform.Find(NEXT_BTN).gameObject;
        mainText = mainContent.GetComponentInChildren<Text>();
        mainImage = mainContent.GetComponentInChildren<Image>();
        if (mainText == null)
        {
            Debug.LogError("NOT FOUND!!!");
        }


        //progressIndicator = gameObject.transform.Find(PROGRESS_INDIC).gameObject;
    }


    public void LoadNextTask()
    {
        StartCoroutine(LoadNextTaskInternal());
    }

    /// <summary>
    /// Call this, to indecate the user, that a new task is loaded.
    /// </summary>
    public void EnableTransition()
    {
        mainContent.SetActive(false);
        nextBtn.SetActive(false);
        EnableProgressIndicator();
    }

    /// <summary>
    /// If the new task is available (e.g. received over network), add it to the wizard with this method.
    /// This method will force the ProgressIndicator to close and after that show the instructions in the dialog.
    /// </summary>
    /// <param name="task"></param>
    [Obsolete("Use the LoadNextTask instead")]
    public void SetNextTask(WizardTask task)
    {
        //TODO handle null as finish or make specific finished task!
        nextTask = task;
        StartCoroutine(DisableProgressIndicator());
    }


    private void EnableProgressIndicator()
    {
        Debug.Log("Enable Transition");
        ProgressIndicator.Instance.Open(IndicatorStyleEnum.AnimatedOrbs, ProgressStyleEnum.None, ProgressMessageStyleEnum.Visible, "Wait for next task...");
    }

    private IEnumerator DisableProgressIndicator()
    {
        Debug.Log("Closing Progress...");
        ProgressIndicator.Instance.SetMessage("Task Loaded");

        // Close the loading dialog
        // ProgressIndicator.Instance.IsLoading will report true until its 'Closing' animation has ended
        // This typically takes about 1 second
        ProgressIndicator.Instance.Close();
        while (ProgressIndicator.Instance.IsLoading)
        {
            yield return null;
        }
        Debug.Log("Closing done...");


        yield break;
    }

    private IEnumerator LoadNextTaskInternal()
    {
        Debug.Log("Loading next task...");
        EnableTransition();
        yield return new WaitForSeconds(3);

        nextTask = WizardTaskManager.Instance.GetNextTask();
        Debug.Log("Task Loaded!");

        UpdateContent();

        yield return DisableProgressIndicator();

        ActivateMainView();

        //Waits only one frame
        //yield return null;

        //break indicates that there are no more stmts coming
        yield break;
    }

    private void ActivateMainView()
    {
        Debug.Log("Activate Main View");
        mainContent.SetActive(true);
        nextBtn.SetActive(true);
    }

    private void UpdateContent()
    {
        Debug.Log("Update Content with new Stuff");
        mainText.text = nextTask.Instruction;

    }

}
