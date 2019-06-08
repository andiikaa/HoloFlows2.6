using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Processes.Proteus.Rest.Model
{
    /// <summary>
    /// Defines PortInstanceType
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PortInstanceTypeEnum
    {

        /// <summary>
        /// Enum StartDataPortInstance for value: StartDataPortInstance
        /// </summary>
        [EnumMember(Value = "StartDataPortInstance")]
        StartDataPortInstance = 1,

        /// <summary>
        /// Enum EndDataPortInstance for value: EndDataPortInstance
        /// </summary>
        [EnumMember(Value = "EndDataPortInstance")]
        EndDataPortInstance = 2,

        /// <summary>
        /// Enum StartControlPortInstance for value: StartControlPortInstance
        /// </summary>
        [EnumMember(Value = "StartControlPortInstance")]
        StartControlPortInstance = 3,

        /// <summary>
        /// Enum EndControlPortInstance for value: EndControlPortInstance
        /// </summary>
        [EnumMember(Value = "EndControlPortInstance")]
        EndControlPortInstance = 4,

        /// <summary>
        /// Enum EscalationPortInstance for value: EscalationPortInstance
        /// </summary>
        [EnumMember(Value = "EscalationPortInstance")]
        EscalationPortInstance = 5
    }
}
