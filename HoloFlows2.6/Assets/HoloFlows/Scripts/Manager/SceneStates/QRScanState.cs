using HoloFlows.Client;
using HoloFlows.Devices;
using HoloFlows.Model;
using HoloFlows.ObjectDetection;
using HoloFlows.Wizard;
using System;
using System.Collections;
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
            DeviceManager.Instance.GetDeviceInfo(data.ThingId, info =>
            {
                string itemName = GetSomeItemName(info);
                sceneManager.StartCoroutine(CheckIfDeviceIsAlreadyPresent(data, itemName));
            });
        }

        protected override ApplicationState GetApplicationState() { return ApplicationState.QRScan; }

        private IEnumerator CheckIfDeviceIsAlreadyPresent(QRCodeData data, string itemName)
        {

            if (itemName == null)
            {
                MoveToWizard(data);
                yield break;
            }

            //TODO the check should go to the sal
            var request = new SingleItemRequest(Settings.OPENHAB_URI, itemName, (item, isNotFound) =>
            {
                if (isNotFound)
                {
                    MoveToWizard(data);
                    return;
                }

                if (item != null)
                {
                    JumpToEdit(data);
                }
                else
                {
                    Debug.LogError("Something wrong connecting openhab...");
                    JumpToEdit(data);
                }

            });

            yield return request.ExecuteRequest();

        }

        private string GetSomeItemName(DeviceInfo data)
        {
            if (data == null)
            {
                Debug.LogError("no data while checking for existing items");
                return null;
            }
            if (data.Functionalities != null && data.Functionalities.Length > 0) return data.Functionalities[0].ItemId;
            if (data.States != null && data.States.Length > 0) return data.States[0].ItemId;
            Debug.LogErrorFormat("no item found for data '{0}'", data.Uid);
            return null;
        }

        private void MoveToWizard(QRCodeData data)
        {
            WizardState wState = new WizardState(sceneManager, data);
            SetNewState(wState);
            wState.StartWizard();
            Debug.Log("Switched to WizardState");
        }

        private void JumpToEdit(QRCodeData data)
        {
            //TODO destroy wizard here?
            DeviceSpawner.Instance.SpawnDevice(data.ThingId, go =>
            {
                //in case device could not be spawned
                if (go == null)
                {
                    ShowAllManagedObjects();
                    SetNewState(new ControlState(sceneManager));
                    Debug.Log("Switched to ControlState");
                    return;
                }

                //the spawned device is in placing mode
                ShowAllManagedObjects();
                EnablePlacingModeForManagedObjects(true);
                PlaceTheGameObject(go);
                SetNewState(new EditState(sceneManager));
                Debug.Log("Switched to EditState");
            });
        }
    }
}
