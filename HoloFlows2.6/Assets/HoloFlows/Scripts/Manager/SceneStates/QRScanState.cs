using HoloFlows.ObjectDetection;
using HoloFlows.Wizard;
using System;
using UnityEngine;

namespace HoloFlows.Manager.SceneStates
{
    internal class QRScanState : AppState
    {
        private readonly GameObject scanInterface;

        public QRScanState(HoloFlowSceneManager sceneManager, GameObject scanInterface) : base(sceneManager)
        {
            if (scanInterface == null)
            {
                throw new ArgumentNullException("scanInterface could not be null for QRScanState");
            }

            this.scanInterface = scanInterface;
        }

        public override void SwitchToWizard(QRCodeData data)
        {
            //sceneManager.InternalDestroy(scanInterface);
            scanInterface.SetActive(false);
            if (!data.IsValid)
            {
                SetNewState(new ControlState(sceneManager));
                Debug.Log("Switched to ControlState");
                return;
            }

            WizardTaskManager.Instance.AddLastScannedData(data);
            WizardState wState = new WizardState(sceneManager, data);
            SetNewState(wState);
            wState.StartWizard();
            Debug.Log("Switched to WizardState");
        }

        protected override ApplicationState GetApplicationState() { return ApplicationState.QRScan; }
    }
}
