using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Processes.Proteus.Rest.Model
{
    /// <summary>
    /// Defines DataTypeInstanceType
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DataTypeInstanceTypeEnum
    {

        /// <summary>
        /// Enum StringTypeInstance for value: StringTypeInstance
        /// </summary>
        [EnumMember(Value = "StringTypeInstance")]
        StringTypeInstance = 1,

        /// <summary>
        /// Enum BooleanTypeInstance for value: BooleanTypeInstance
        /// </summary>
        [EnumMember(Value = "BooleanTypeInstance")]
        BooleanTypeInstance = 2,

        /// <summary>
        /// Enum ComplexTypeInstance for value: ComplexTypeInstance
        /// </summary>
        [EnumMember(Value = "ComplexTypeInstance")]
        ComplexTypeInstance = 3,

        /// <summary>
        /// Enum IntegerTypeInstance for value: IntegerTypeInstance
        /// </summary>
        [EnumMember(Value = "IntegerTypeInstance")]
        IntegerTypeInstance = 4,

        /// <summary>
        /// Enum DoubleTypeInstance for value: DoubleTypeInstance
        /// </summary>
        [EnumMember(Value = "DoubleTypeInstance")]
        DoubleTypeInstance = 5,

        /// <summary>
        /// Enum ListTypeInstance for value: ListTypeInstance
        /// </summary>
        [EnumMember(Value = "ListTypeInstance")]
        ListTypeInstance = 6,

        /// <summary>
        /// Enum SetTypeInstance for value: SetTypeInstance
        /// </summary>
        [EnumMember(Value = "SetTypeInstance")]
        SetTypeInstance = 7
    }
}
