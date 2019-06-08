using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Processes.Proteus.Rest.Model
{
    /// <summary>
    /// Defines HumanTaskUseCase
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum HumanTaskUseCaseEnum
    {

        /// <summary>
        /// Enum UNIVERSAL for value: UNIVERSAL
        /// </summary>
        [EnumMember(Value = "UNIVERSAL")]
        UNIVERSAL = 1,

        /// <summary>
        /// Enum COFFEE for value: COFFEE
        /// </summary>
        [EnumMember(Value = "COFFEE")]
        COFFEE = 2,

        /// <summary>
        /// Enum HEATING for value: HEATING
        /// </summary>
        [EnumMember(Value = "HEATING")]
        HEATING = 3,

        /// <summary>
        /// Enum ORDER for value: ORDER
        /// </summary>
        [EnumMember(Value = "ORDER")]
        ORDER = 4,

        /// <summary>
        /// Enum PLANTS for value: PLANTS
        /// </summary>
        [EnumMember(Value = "PLANTS")]
        PLANTS = 5,

        /// <summary>
        /// Enum BELL for value: BELL
        /// </summary>
        [EnumMember(Value = "BELL")]
        BELL = 6,

        /// <summary>
        /// Enum HEALTH for value: HEALTH
        /// </summary>
        [EnumMember(Value = "HEALTH")]
        HEALTH = 7
    }
}
