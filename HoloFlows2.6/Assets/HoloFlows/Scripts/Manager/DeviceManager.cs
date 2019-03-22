
using HoloToolkit.Unity;
using System.Collections.Generic;

public class DeviceManager : Singleton<DeviceManager>
{

    public Dictionary<string, DeviceInfo> DeviceInfos { get; private set; } = new Dictionary<string, DeviceInfo>();

    void Start()
    {
        AddDemoDevices();
    }

    /// <summary>
    /// The "Thing"
    /// </summary>
    public class DeviceInfo
    {
        public string Uid { get; set; }
        public string DisplayName { get; set; }
        public DeviceFunctionality[] Functionalities { get; set; }

    }

    /// <summary>
    /// The "Item" which can receive commands, like "ON"
    /// </summary>
    public class DeviceFunctionality
    {
        public string FunctionalityType { get; set; }
        public string ItemId { get; set; }
        public DeviceCommand[] Commands { get; set; }
    }

    /// <summary>
    /// The "Item" which represents a state, such a temperature.
    /// </summary>
    public class DeviceState
    {
        public string StateType { get; set; }
        public string RealStateValue { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
    }

    public class UnitOfMeasure
    {
        public string UnitName { get; set; }
        public string PrefixSymbol { get; set; }
    }

    public class DeviceCommand
    {
        public string Name { get; set; }
        public string RealCommandName { get; set; }
        public string CommandType { get; set; }
    }

    private void AddDemoDevices()
    {
        var homematicDimmer = CreateHomematicDimmer();
        DeviceInfos.Add(homematicDimmer.Uid, homematicDimmer);
    }

    private DeviceInfo CreateHomematicDimmer()
    {
        //TODO Devices for dev stuff
        DeviceInfo homematicDimmer = new DeviceInfo()
        {
            Uid = "homematic_dimmer_1",
            DisplayName = "Homematic Dimmer"
        };

        DeviceCommand onCommand = new DeviceCommand()
        {
            Name = "DEFAULT_ON_COMMAND",
            RealCommandName = "ON",
            CommandType = "dogont:OnCommand"
        };

        DeviceCommand offCommand = new DeviceCommand()
        {
            Name = "DEFAULT_OFF_COMMAND",
            RealCommandName = "OFF",
            CommandType = "dogont:OffCommand"
        };

        DeviceCommand upCommand = new DeviceCommand()
        {
            Name = "DEFAULT_UP_COMMAND",
            RealCommandName = "UP",
            CommandType = "dogont:UpCommand"
        };

        DeviceCommand downCommand = new DeviceCommand()
        {
            Name = "DEFAULT_DOWN_COMMAND",
            RealCommandName = "DOWN",
            CommandType = "dogont:DownCommand"
        };

        DeviceFunctionality f1 = new DeviceFunctionality()
        {
            FunctionalityType = "dogont:LevelControlFunctionality",
            ItemId = "homematic_dimmer_dimmer_1",
            Commands = new[] { onCommand, offCommand, upCommand, downCommand }
        };

        homematicDimmer.Functionalities = new[] { f1 };
        return homematicDimmer;
    }
}

