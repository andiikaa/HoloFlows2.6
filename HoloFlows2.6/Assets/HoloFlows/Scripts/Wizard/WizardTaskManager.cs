using HoloFlows.ObjectDetection;
using HoloToolkit.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HoloFlows.Wizard
{

    /// <summary>
    /// Handles all Tasks in FIFO Order.
    /// </summary>
    public class WizardTaskManager : Singleton<WizardTaskManager>
    {

        private List<WizardTask> tasks = new List<WizardTask>();
        private QRCodeData qrCodeData;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddLastScannedData(QRCodeData scannedData)
        {
            if (!scannedData.IsValid)
            {
                Debug.LogError("Invalid qr code data added to the WizardTaskManager");
                return;
            }
            qrCodeData = scannedData;
        }

        public void LoadWorkflowForLastScan(Action<bool> workflowReady)
        {
            tasks.Clear();
            //send request
            //wait for first human task
            //add content
            tasks.AddRange(WizardDemo.CreateCompleteBulbExample());
            workflowReady?.Invoke(true);
        }

        public void AddTask(WizardTask task)
        {
            tasks.Add(task);
        }

        public WizardTask GetNextTask()
        {
            if (!tasks.Any())
            {
                return null;
            }

            WizardTask task = tasks[0];
            tasks.RemoveAt(0);
            return task;
        }

    }
}
