using HoloFlows.ButtonScripts;
using HoloFlows.Manager;
using HoloFlows.Processes;
using System.Collections;
using UnityEngine;

namespace HoloFlows
{
    public class AppControl : MonoBehaviour, IAppStateListener
    {
        private AppStateButton controlBtn;
        private AppStateButton editBtn;
        private AppStateButton scanBtn;


        public void Start()
        {
            controlBtn = gameObject.transform.Find("ControlModeBtn").GetComponent<AppStateButton>();
            editBtn = gameObject.transform.Find("EditModeBtn").GetComponent<AppStateButton>();
            scanBtn = gameObject.transform.Find("ScanModeBtn").GetComponent<AppStateButton>();

            controlBtn.ButtonType = AppStateButtonType.CONTROL;
            editBtn.ButtonType = AppStateButtonType.EDIT;
            scanBtn.ButtonType = AppStateButtonType.SCAN;

            HoloFlowSceneManager.Instance.RegisterAppStateListener(this);

            //sets the initial state
            //fast dirty hack 
            try
            {
                AppStateChanged(HoloFlowSceneManager.Instance.ApplicationState);
            }
            catch (System.NullReferenceException)
            {
                Debug.LogWarning("The current app state is not availaible. Using Control State instead.");
                AppStateChanged(ApplicationState.Control);
            }

            StartCoroutine(RequestHumanTasks());
        }

        //TODO remove this from here
        private IEnumerator RequestHumanTasks()
        {
            ProteusRestClient client = new ProteusRestClient();
            yield return client.GetHumanTaskList(r =>
            {
                Debug.LogFormat("Has HumanTask Error: {0}", r.HasError);
            });
        }

        public void AppStateChanged(ApplicationState appState)
        {
            switch (appState)
            {
                case ApplicationState.Control:
                    //Disable Control
                    SetActiveIfNeeded();
                    controlBtn.VisualDisable();
                    controlBtn.MarkAsCurrentState();

                    editBtn.VisualEnable();
                    editBtn.UnmarkAsCurrentState();

                    scanBtn.VisualEnable();
                    scanBtn.UnmarkAsCurrentState();
                    break;
                case ApplicationState.Edit:
                    //Disable Edit and Scan
                    SetActiveIfNeeded();
                    controlBtn.VisualEnable();
                    controlBtn.UnmarkAsCurrentState();

                    editBtn.VisualDisable();
                    editBtn.MarkAsCurrentState();

                    scanBtn.VisualDisable();
                    scanBtn.UnmarkAsCurrentState();
                    break;
                case ApplicationState.QRScan:
                    //Disable complete view
                    gameObject.SetActive(false);
                    break;
                case ApplicationState.Wizard:
                    //Disable complete view
                    gameObject.SetActive(false);
                    break;
                default:
                    SetActiveIfNeeded();
                    editBtn.VisualEnable();
                    scanBtn.VisualEnable();
                    controlBtn.VisualEnable();

                    editBtn.UnmarkAsCurrentState();
                    scanBtn.UnmarkAsCurrentState();
                    controlBtn.UnmarkAsCurrentState();

                    break;
            }
        }

        private void SetActiveIfNeeded()
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
        }

    }
}
