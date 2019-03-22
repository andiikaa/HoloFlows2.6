using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Some of the possible commands are listed here:
/// https://www.openhab.org/docs/concepts/items.html
/// </summary>
public class DefaultDeviceButtonBehavior : TapSoundButton
{

    private string _commandDisplayName = null;

    /// <summary>
    /// DeviceId e.g. the OpenHab ItemName
    /// </summary>
    public string DeviceId { get; set; }

    /// <summary>
    /// The real command name, which should be send to the middleware. 
    /// E.g. "1" for on and "0" for off.
    /// </summary>
    public string RealCommandName { get; set; }

    /// <summary>
    /// The display name for the command button in the ui.
    /// E.g. "ON" or "PLAY"
    /// </summary>
    public string CommandDisplayName
    {
        get { return _commandDisplayName; }
        set
        {
            _commandDisplayName = value;
            SetDisplayName();
        }
    }

    /// <summary>
    /// Checks if the given values for this button are valid.
    /// If not, the 
    /// </summary>
    public bool HasMissingValues
    {
        get
        {
            return string.IsNullOrEmpty(DeviceId)
                || string.IsNullOrEmpty(RealCommandName);
        }
    }

    public override void HandleClickEvent(InputClickedEventData eventData)
    {
        if (HasMissingValues)
        {
            Debug.LogError("Command Data is missing!!");
        }
        else
        {
            Debug.LogFormat("Sending Command '{0}' to device {1}", RealCommandName, DeviceId);
        }
    }

    private void SetDisplayName()
    {
        Transform textObj = gameObject.transform.Find("Text");
        if (textObj != null)
        {
            Text text = textObj.GetComponent<Text>();
            text.text = CommandDisplayName;
        }
    }

    new void Start()
    {
        base.Start();
        Debug.Log("Starting default button behavior");
    }
}
