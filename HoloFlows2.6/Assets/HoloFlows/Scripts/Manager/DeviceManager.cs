
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
        public DeviceState[] States { get; set; }
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
        public string ItemId { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
    }

    /// <summary>
    /// UnitType and UnitName could be derived from the instance uri of the unit in the dogont ontology
    /// E.g. http://elite.polito.it/ontologies/ucum-instances.owl#unit/temperature/degree-Celsius
    /// Type = temperature,
    /// Name = degree-Celsius
    /// </summary>
    public class UnitOfMeasure
    {
        public string UnitName { get; set; }
        public string UnitType { get; set; }
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
        var tinkerforgeAmbTemp = CreateTinkerforgeIRTemp();
        DeviceInfos.Add(homematicDimmer.Uid, homematicDimmer);
        DeviceInfos.Add(tinkerforgeAmbTemp.Uid, tinkerforgeAmbTemp);
    }

    private DeviceInfo CreateTinkerforgeIRTemp()
    {
        DeviceInfo tinkerforgeIrTemp = new DeviceInfo()
        {
            Uid = "tinkerforge_irTemp_1",
            DisplayName = "Tinkerforge IR Temp"
        };

        UnitOfMeasure degree = new UnitOfMeasure()
        {
            UnitName = "degree-Celsius",
            UnitType = "temperature",
            PrefixSymbol = "°C"
        };

        DeviceState irTempState = new DeviceState()
        {
            ItemId = "tinkerforge_irTemp_irTemp_1",
            UnitOfMeasure = degree,
            RealStateValue = "20",
            StateType = "dogont:TemperatureState"
        };

        DeviceState ambTempState = new DeviceState()
        {
            ItemId = "tinkerforge_irTemp_ambTemp_1",
            UnitOfMeasure = degree,
            RealStateValue = "15",
            StateType = "dogont:TemperatureState"
        };


        tinkerforgeIrTemp.States = new[] { irTempState, ambTempState };
        return tinkerforgeIrTemp;
    }

    private DeviceInfo CreateHomematicDimmer()
    {
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

