using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Processes.Proteus.Rest.Model
{
    /// <summary>
    /// Defines State
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StateEnum
    {

        /// <summary>
        /// Enum ACTIVE for value: ACTIVE
        /// </summary>
        [EnumMember(Value = "ACTIVE")]
        ACTIVE = 1,

        /// <summary>
        /// Enum INACTIVE for value: INACTIVE
        /// </summary>
        [EnumMember(Value = "INACTIVE")]
        INACTIVE = 2,

        /// <summary>
        /// Enum EXECUTING for value: EXECUTING
        /// </summary>
        [EnumMember(Value = "EXECUTING")]
        EXECUTING = 3,

        /// <summary>
        /// Enum EXECUTED for value: EXECUTED
        /// </summary>
        [EnumMember(Value = "EXECUTED")]
        EXECUTED = 4,

        /// <summary>
        /// Enum PAUSED for value: PAUSED
        /// </summary>
        [EnumMember(Value = "PAUSED")]
        PAUSED = 5,

        /// <summary>
        /// Enum STOPPED for value: STOPPED
        /// </summary>
        [EnumMember(Value = "STOPPED")]
        STOPPED = 6,

        /// <summary>
        /// Enum FAILED for value: FAILED
        /// </summary>
        [EnumMember(Value = "FAILED")]
        FAILED = 7,

        /// <summary>
        /// Enum KILLED for value: KILLED
        /// </summary>
        [EnumMember(Value = "KILLED")]
        KILLED = 8,

        /// <summary>
        /// Enum WAITING for value: WAITING
        /// </summary>
        [EnumMember(Value = "WAITING")]
        WAITING = 9,

        /// <summary>
        /// Enum UNDEPLOYED for value: UNDEPLOYED
        /// </summary>
        [EnumMember(Value = "UNDEPLOYED")]
        UNDEPLOYED = 10,

        /// <summary>
        /// Enum ESCALATED for value: ESCALATED
        /// </summary>
        [EnumMember(Value = "ESCALATED")]
        ESCALATED = 11,

        /// <summary>
        /// Enum DEACTIVATED for value: DEACTIVATED
        /// </summary>
        [EnumMember(Value = "DEACTIVATED")]
        DEACTIVATED = 12
    }
}
