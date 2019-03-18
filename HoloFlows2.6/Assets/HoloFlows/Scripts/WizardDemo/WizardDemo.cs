using HoloToolkit.Unity;

public class WizardDemo : Singleton<WizardDemo>
{
    public void Start()
    {
        WizardTaskManager tm = WizardTaskManager.Instance;
        WizardTask t1 = new WizardTask()
        {
            Name = "Task 1",
            Instruction = "Let´s get started..."
        };
        WizardTask t2 = new WizardTask()
        {
            Name = "Task 2",
            Instruction = "The second task, only 2 left after this."
        };
        WizardTask t3 = new WizardTask()
        {
            Name = "Task 3",
            Instruction = "Only one task remaining. Let´s do this, now!"
        };
        WizardTask t4 = new WizardTask()
        {
            Name = "Task 4",
            Instruction = "Finaly, the last task is waiting for you."
        };
        tm.AddTask(t1);
        tm.AddTask(t2);
        tm.AddTask(t3);
        tm.AddTask(t4);
    }
}

