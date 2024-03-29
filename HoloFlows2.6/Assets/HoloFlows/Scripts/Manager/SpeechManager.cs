﻿using HoloToolkit.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace HoloFlows.Manager
{
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
