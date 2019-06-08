using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Processes.Proteus.Rest.Model
{
    /// <summary>
    /// Defines HumanTaskType
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum HumanTaskTypeEnum
    {

        /// <summary>
        /// Enum HINT for value: HINT
        /// </summary>
        [EnumMember(Value = "HINT")]
        HINT = 1,

        /// <summary>
        /// Enum WARNING for value: WARNING
        /// </summary>
        [EnumMember(Value = "WARNING")]
        WARNING = 2,

        /// <summary>
        /// Enum INTERACTION for value: INTERACTION
        /// </summary>
        [EnumMember(Value = "INTERACTION")]
        INTERACTION = 3,

        /// <summary>
        /// Enum ERROR for value: ERROR
        /// </summary>
        [EnumMember(Value = "ERROR")]
        ERROR = 4
    }
}
