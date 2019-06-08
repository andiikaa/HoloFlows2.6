using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Processes.Proteus.Rest.Model
{
    /// <summary>
    /// Defines PortType
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PortTypeEnum
    {

        /// <summary>
        /// Enum StartDataPort for value: StartDataPort
        /// </summary>
        [EnumMember(Value = "StartDataPort")]
        StartDataPort = 1,

        /// <summary>
        /// Enum EndDataPort for value: EndDataPort
        /// </summary>
        [EnumMember(Value = "EndDataPort")]
        EndDataPort = 2,

        /// <summary>
        /// Enum StartControlPort for value: StartControlPort
        /// </summary>
        [EnumMember(Value = "StartControlPort")]
        StartControlPort = 3,

        /// <summary>
        /// Enum EndControlPort for value: EndControlPort
        /// </summary>
        [EnumMember(Value = "EndControlPort")]
        EndControlPort = 4,

        /// <summary>
        /// Enum EscalationPort for value: EscalationPort
        /// </summary>
        [EnumMember(Value = "EscalationPort")]
        EscalationPort = 5
    }
}
