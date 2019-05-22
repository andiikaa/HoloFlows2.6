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
        //WizardTaskManager tm = WizardTaskManager.Instance;
        //List<WizardTask> hueTasks = CreateCompleteBulbExample();
        //foreach (WizardTask t in hueTasks)
        //{
        //    tm.AddTask(t);
        //}
    }

    public static List<WizardTask> CreateCompleteBulbExample()
    {
        List<WizardTask> tasks = new List<WizardTask>();

        // WfMS Task: Check if Bridge is present (for this example it is not!)

        WizardTask Bt1 = new WizardTask()
        {
            Name = "Welcome",
            Instruction = "The following tasks, will setup the hue Bridge.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Hue/1-hello.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Hue/1.jpg"
        };
        WizardTask Bt2 = new WizardTask()
        {
            Name = "Connect Cables",
            Instruction = "Plugin the Network and Power Cable to the Bridge.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Hue/2-cable.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Hue/2.jpg"
        };

        WizardTask Bt3 = new WizardTask()
        {
            Name = "Ensure Lights",
            Instruction = "Ensure that the status lights are flashing.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Hue/3-status.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Hue/3.jpg"
        };


        // WfMS Task: Autodiscovery
        // WfMS Task: InitHue Connection

        WizardTask Bt4 = new WizardTask()
        {
            Name = "Press Connect Button",
            Instruction = "Press the 'Philips' Button on the Bridge.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Hue/4-button.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Hue/4.jpg"
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
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Hue/5.jpg"
        };

        WizardTask t3 = new WizardTask()
        {
            Name = "Switch on Power",
            Instruction = "Ensure power is switched on.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Hue/6-switched-on.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Hue/6.jpg"
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

    public static List<WizardTask> CreateTinkerforgeIRExample()
    {
        List<WizardTask> tasks = new List<WizardTask>();

        WizardTask t1 = new WizardTask()
        {
            Name = "Hallo",
            Instruction = "Die folgende Schritte richten das Tinkerforge Infrarot Temperatur Bricklet ein.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Tinkerforge/IR/1.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Tinkerforge/IR/1.png"
        };

        WizardTask t2 = new WizardTask()
        {
            Name = "WIFI Module verbinden",
            Instruction = "Verbinde das WIFI Modul mit dem Masterbrick. Achte auf die korrekte Ausrichtung des WIFI Moduls.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Tinkerforge/IR/2.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Tinkerforge/IR/2.png"
        };

        WizardTask t3 = new WizardTask()
        {
            Name = "An PC anschließen",
            Instruction = "Schließe nun den Masterbrick mit Hilfe eines USB Kabels an deinen PC an.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Tinkerforge/IR/3.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Tinkerforge/IR/3.png"
        };

        WizardTask t4 = new WizardTask()
        {
            Name = "BrickViewer starten",
            Instruction = "Starte das Programm 'BrickViewer', welches auf dem Desktop zu finden ist.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Tinkerforge/IR/4.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Tinkerforge/IR/4.png"
        };

        WizardTask t5 = new WizardTask()
        {
            Name = "Verbinden",
            Instruction = "Überprüfe die Werte im Reiter 'Setup'. Drücke 'Connect' um dich mit dem Bricklet zu verbinden.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Tinkerforge/IR/5.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Tinkerforge/IR/5.png"
        };

        WizardTask t6 = new WizardTask()
        {
            Name = "Parameter eingeben",
            Instruction = "Wechsle nun in den Reiter 'Master Brick 2.1' und trage im Abschnitt 'Client Mode', die dargestellten Parameter ein.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Tinkerforge/IR/6.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Tinkerforge/IR/6.png"
        };

        WizardTask t7 = new WizardTask()
        {
            Name = "Sichern",
            Instruction = "Klicke nun auf 'Save WIFI Configuration', um die Einstellungen zu speichern.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Tinkerforge/IR/7.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Tinkerforge/IR/7.png"
        };

        WizardTask t8 = new WizardTask()
        {
            Name = "Trennen",
            Instruction = "Wechsel zurück in den Reiter 'Setup' und trenne die Verbindnung über Klick auf 'Disconnect'.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Tinkerforge/IR/8.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Tinkerforge/IR/8.png"
        };

        WizardTask t9 = new WizardTask()
        {
            Name = "Temperatur Bricklet anschließen",
            Instruction = "Ziehe das USB Kabel von deinem Master Brick ab und verbinde das Temperatur Bricklet mit dem Master.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Tinkerforge/IR/9.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Tinkerforge/IR/9.png"
        };

        WizardTask t10 = new WizardTask()
        {
            Name = "Stromversorgung anschließen",
            Instruction = "Verbinde den Master wieder mit einem USB Kabel, um die Stromversorgung zu gewährleisten.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Tinkerforge/IR/10.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Tinkerforge/IR/10.png"
        };

        //WizardTask t11 = new WizardTask()
        //{
        //    Name = "Erfolgreich",
        //    Instruction = "Die Einrichtung vom Infrarot Temperatur Bricklet war erfolgreich.",
        //    AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Tinkerforge/IR/11.wav",
        //    ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Tinkerforge/IR/11.png"
        //};

        tasks.Add(t1);
        tasks.Add(t2);
        tasks.Add(t3);
        tasks.Add(t4);
        tasks.Add(t5);
        tasks.Add(t6);
        tasks.Add(t7);
        tasks.Add(t8);
        tasks.Add(t9);
        tasks.Add(t10);
        //tasks.Add(t11);
        return tasks;
    }

    public static List<WizardTask> CreateTinkerforgeAmbientLight()
    {
        var tasks = new List<WizardTask>();

        WizardTask t1 = new WizardTask()
        {
            Name = "Hallo",
            Instruction = "Die folgenden Schritte richten das Tinkerforge Helligkeits-Sensor Bricklet ein.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Tinkerforge/AmbientLight/1.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Tinkerforge/AmbientLight/1.png"
        };

        WizardTask t2 = new WizardTask()
        {
            Name = "Entfernen",
            Instruction = "Entferne den Master Brick vom USB Kabel.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Tinkerforge/AmbientLight/2.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Tinkerforge/AmbientLight/2.png"
        };

        WizardTask t3 = new WizardTask()
        {
            Name = "Anschließen",
            Instruction = "Schließe das Helligkeits-Sensor Bricklet an.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Tinkerforge/AmbientLight/3.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Tinkerforge/AmbientLight/3.png"
        };

        WizardTask t4 = new WizardTask()
        {
            Name = "USB anschließen",
            Instruction = "Verbinde den Master Brick mit dem USB Kabel.",
            AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Tinkerforge/AmbientLight/4.wav",
            ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Tinkerforge/AmbientLight/4.png"
        };

        //WizardTask t5 = new WizardTask()
        //{
        //    Name = "Erfolgreich",
        //    Instruction = "Die Einrichtung vom Helligkeits-Sensor Bricklet war erfolgreich.",
        //    AudioUri = "file://Assets/HoloFlows/Resources/Wizard/Audio/Tinkerforge/AmbientLight/5.wav",
        //    ImageUri = "file://Assets/HoloFlows/Resources/Wizard/Image/Tinkerforge/AmbientLight/5.png"
        //};

        tasks.Add(t1);
        tasks.Add(t2);
        tasks.Add(t3);
        tasks.Add(t4);
        //tasks.Add(t5);
        return tasks;
    }
}

