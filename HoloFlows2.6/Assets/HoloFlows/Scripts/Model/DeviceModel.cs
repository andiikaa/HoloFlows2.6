namespace HoloFlows.Model
{
    /// <summary>
    /// The "Thing"
    /// </summary>
    public class DeviceInfo
    {
        public string Uid { get; set; }
        public string DisplayName { get; set; }
        public DeviceFunctionality[] Functionalities { get; set; }
        public DeviceState[] States { get; set; }
        public GroupBox[] GroupBoxes { get; set; }
        /// <summary>
        /// Info wether the device is currently in hf integrated or not
        /// </summary>
        public bool IsInHoloFlows { get; set; }
    }

    /// <summary>
    /// The "Item" which can receive commands, like "ON"
    /// </summary>
    public class DeviceFunctionality
    {
        public string Label { get; set; }
        public string FunctionalityType { get; set; }
        public string ItemId { get; set; }
        public DeviceCommand[] Commands { get; set; }
        public GroupBox GroupBox { get; set; }
    }

    /// <summary>
    /// The "Item" which represents a state, such a temperature.
    /// </summary>
    public class DeviceState
    {
        public string Label { get; set; }
        public string StateType { get; set; }
        public string RealStateValue { get; set; }
        public string ItemId { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public GroupBox GroupBox { get; set; }
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
        public string Label { get; set; }
    }

    /// <summary>
    /// GroupBoxes define grouping for functionality and states
    /// </summary>
    public class GroupBox
    {
        public string Name { get; set; }
        public string IconName { get; set; }
    }
}
