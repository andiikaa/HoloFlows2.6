using HoloToolkit.Unity;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Handles all Tasks in FIFO Order.
/// </summary>
public class WizardTaskManager : Singleton<WizardTaskManager>
{

    private List<WizardTask> tasks = new List<WizardTask>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
