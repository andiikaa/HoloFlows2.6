using HoloToolkit.UX.Progress;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//TODO initialization!
public class WizardDialog : MonoBehaviour
{
    //child object names
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
    private bool isFinished = false;

    void Start()
    {
        isFinished = false;
        mainContent = gameObject.transform.Find(MAIN_CONTENT).gameObject;
        nextBtn = gameObject.transform.Find(NEXT_BTN).gameObject;
        mainText = mainContent.GetComponentInChildren<Text>();
        mainImage = mainContent.GetComponentInChildren<Image>();
    }

    /// <summary>
    /// Call this to load the next task into the wizard. If no task is left, a final message will be shown.
    /// The next button will say close and on the next click the window is closed.
    /// </summary>
    public void LoadNextTask()
    {
        if (isFinished)
        {
            Debug.Log("TODO Close the window");
            //TODO close window
            CloseDialog();
        }
        else
        {
            StartCoroutine(LoadNextTaskInternal());
        }
    }

    /// <summary>
    /// Call this, to indicate the user, that a new task is loaded.
    /// </summary>
    private void EnableTransition()
    {
        mainContent.SetActive(false);
        nextBtn.SetActive(false);
        EnableProgressIndicator();
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

        //TODO with remote process engine it is better to use a metatask which indicates the finish
        nextTask = WizardTaskManager.Instance.GetNextTask();
        Debug.Log("Task Loaded!");

        UpdateContent();

        yield return DisableProgressIndicator();

        ActivateMainView();

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
        if (nextTask == null)
        {
            UpdateContentWithFinsihed();
        }
        else
        {
            UpdateContentWithNextTask();
        }
    }

    private void UpdateContentWithFinsihed()
    {
        isFinished = true;
        mainText.text = "You have finished the assembly. Close this dialog to interact with you new device.";
        nextBtn.GetComponentInChildren<Text>().text = "Close";
    }

    private void UpdateContentWithNextTask()
    {
        mainText.text = nextTask.Instruction;
    }

    private void CloseDialog()
    {
        //gameObject.SetActive(false);
        GameObject.Destroy(gameObject);
    }

}
