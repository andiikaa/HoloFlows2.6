using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Processes.Proteus.Rest.Model
{
    /// <summary>
    /// Defines DataTypeType
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DataTypeTypeEnum
    {

        /// <summary>
        /// Enum StringType for value: StringType
        /// </summary>
        [EnumMember(Value = "StringType")]
        StringType = 1,

        /// <summary>
        /// Enum BooleanType for value: BooleanType
        /// </summary>
        [EnumMember(Value = "BooleanType")]
        BooleanType = 2,

        /// <summary>
        /// Enum ComplexType for value: ComplexType
        /// </summary>
        [EnumMember(Value = "ComplexType")]
        ComplexType = 3,

        /// <summary>
        /// Enum IntegerType for value: IntegerType
        /// </summary>
        [EnumMember(Value = "IntegerType")]
        IntegerType = 4,

        /// <summary>
        /// Enum DoubleType for value: DoubleType
        /// </summary>
        [EnumMember(Value = "DoubleType")]
        DoubleType = 5,

        /// <summary>
        /// Enum ListType for value: ListType
        /// </summary>
        [EnumMember(Value = "ListType")]
        ListType = 6,

        /// <summary>
        /// Enum SetType for value: SetType
        /// </summary>
        [EnumMember(Value = "SetType")]
        SetType = 7
    }
}
