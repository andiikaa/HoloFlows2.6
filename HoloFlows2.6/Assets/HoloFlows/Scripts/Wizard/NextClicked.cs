using HoloToolkit.Unity.InputModule;

public class NextClicked : TapSoundButton
{

    WizardDialog dialog;

    new public void Start()
    {
        base.Start();
        dialog = transform.GetComponentInParent<WizardDialog>();
    }


    public override void HandleClickEvent(InputClickedEventData eventData)
    {
        dialog.LoadNextTask();
    }

}
