using HoloToolkit.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace HoloFlows.Manager
{
    /// <summary>
    /// Speechmanager handles the speech input of the user to switch between the app states
    /// </summary>
    public class SpeechManager : Singleton<SpeechManager>
    {
        private HoloFlowSceneManager sceneManager;

        private Dictionary<string, Action> keywords = new Dictionary<string, Action>();
        private KeywordRecognizer keywordRecognizer;


        // Use this for initialization
        void Start()
        {
            sceneManager = HoloFlowSceneManager.Instance;
            InitKeywords();
            InitKeywordRecognizer();
            Debug.Log("SpeechManager started");
        }

        void OnDisable()
        {
            if (keywordRecognizer != null && keywordRecognizer.IsRunning)
            {
                keywordRecognizer.Stop();
            }
        }

        private void InitKeywordRecognizer()
        {
            //FIXME
            //Sometimes init failes with the following error:
            //'Speech recognition is not supported on this machine.'
            //Also the speech is sometimes not recognized anymore after using the app for a while.
            //Restarting the HoloLens will fix the problem only for a few moments.
            //SpeechManager was replaced with a ControlPanel. 

            try
            {
                keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray(), ConfidenceLevel.Low);
                keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
                keywordRecognizer.Start();
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("Could not init the KeywordRecognizer. Speech is not recognized!\n{0}", ex.Message);
            }
        }

        private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            Debug.LogFormat("phrase recognized: '{0}'", args.text);
            Action keywordAction;
            // if the keyword recognized is in our dictionary, call that Action.
            if (keywords.TryGetValue(args.text, out keywordAction))
            {
                keywordAction.Invoke();
            }
        }

        private void InitKeywords()
        {
            keywords.Add("scan device", ActionScanDevice);
            keywords.Add("edit mode", ActionEditMode);
            keywords.Add("control mode", ActionControlMode);
            keywords.Add("exit", ActionEditMode);
        }

        private void ActionControlMode()
        {
            sceneManager.SwitchToControl();
        }

        private void ActionEditMode()
        {
            sceneManager.SwitchToEdit();
        }

        private void ActionScanDevice()
        {
            sceneManager.SwitchToQRScan();
        }

    }
}
