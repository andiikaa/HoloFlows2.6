﻿using HoloFlows.ButtonScripts;
using HoloToolkit.Unity.InputModule;

namespace HoloFlows.Wizard
{

    public class NextClicked : TapSoundButton
    {

        WizardDialog dialog;

        public override void Start()
        {
            base.Start();
            dialog = transform.GetComponentInParent<WizardDialog>();
        }


        public override void HandleClickEvent(InputClickedEventData eventData)
        {
            dialog.LoadNextTask();
        }

    }
}
