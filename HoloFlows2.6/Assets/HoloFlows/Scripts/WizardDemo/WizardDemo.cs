using HoloFlows.Wizard;
using HoloToolkit.Unity;
using System.Collections.Generic;

/// <summary>
/// Demo Wizard for dev
/// </summary>
public class WizardDemo : Singleton<WizardDemo>
{

    //TODO maybe checkout the speech stuff
    //https://abhijitjana.net/2017/01/02/using-text-to-speech-in-your-holographic-app/
    public void Start()
    {
        WizardTaskManager tm = WizardTaskManager.Instance;
        List<WizardTask> hueTasks = CreateCompleteBulbExample();
        foreach (WizardTask t in hueTasks)
        {
            tm.AddTask(t);
        }
    }

    private static List<WizardTask> CreateCompleteBulbExample()
    {
        List<WizardTask> tasks = new List<WizardTask>();

        // WfMS Task: Check if Bridge is present (for this example it is not!)

        WizardTask Bt1 = new WizardTask()
        {
            Name = "Welcome",
            Instruction = "The following tasks, will setup the hue Bridge.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Hue/1-hello.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Hue/bulb.jpg"
        };
        WizardTask Bt2 = new WizardTask()
        {
            Name = "Connect Cables",
            Instruction = "Plugin the Network and Power Cable to the Bridge.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Hue/2-cable.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Hue/bulb.jpg"
        };

        WizardTask Bt3 = new WizardTask()
        {
            Name = "Ensure Lights",
            Instruction = "Ensure that the status lights are flashing.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Hue/3-status.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Hue/bulb.jpg"
        };


        // WfMS Task: Autodiscovery
        // WfMS Task: InitHue Connection

        WizardTask Bt4 = new WizardTask()
        {
            Name = "Press Connect Button",
            Instruction = "Press the 'Philips' Button on the Bridge.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Hue/4-button.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Hue/bulb.jpg"
        };

        // WfMS Task: autoapprove bridge

        //WizardTask Bt4 = new WizardTask()
        //{
        //    Name = "You´re done",
        //    Instruction = "The Hue Bridge is ready to use."
        //};


        //WizardTask t1 = new WizardTask()
        //{
        //    Name = "Welcome",
        //    Instruction = "The following tasks, will setup the hue Bulb."
        //};

        WizardTask t2 = new WizardTask()
        {
            Name = "Insert Bulb",
            Instruction = "Insert the Hue Bulb to your lamp.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Hue/5-bulb.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Hue/bulb.jpg"
        };

        WizardTask t3 = new WizardTask()
        {
            Name = "Switch on Power",
            Instruction = "Ensure power is switched on.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Hue/6-switched-on.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Hue/bulb.jpg"
        };

        // WfMS Task: GetBridge id
        // WfMS Task: Create Items

        tasks.Add(Bt1);
        tasks.Add(Bt2);
        tasks.Add(Bt3);
        tasks.Add(Bt4);
        tasks.Add(t2);
        tasks.Add(t3);

        return tasks;
    }
}

