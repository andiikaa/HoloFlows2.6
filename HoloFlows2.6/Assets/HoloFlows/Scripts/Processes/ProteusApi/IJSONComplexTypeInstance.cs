/* 
 * PROtEUS REST API
 *
 * Accessing the PROtEUS runtime via REST
 *
 * OpenAPI spec version: v1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Processes.Proteus.Rest.Model
{
    /// <summary>
    /// IJSONComplexTypeInstance
    /// </summary>
    [DataContract]
    public partial class IJSONComplexTypeInstance : IJSONTypeInstance,  IEquatable<IJSONComplexTypeInstance>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IJSONComplexTypeInstance" /> class.
        /// </summary>
        /// <param name="SubTypes">SubTypes.</param>
        //public IJSONComplexTypeInstance(List<IJSONTypeInstance> SubTypes = default(List<IJSONTypeInstance>), string Name = default(string), IJSONType DataType = default(IJSONType), string ValueString = default(string), Object ValueAsObject = default(Object), string TypeClass = default(string), DataTypeInstanceTypeEnum? DataTypeInstanceType = default(DataTypeInstanceTypeEnum?), string ValueStringComplete = default(string), string DataTypeID = default(string), string InstanceID = default(string)) : base(Name, DataType, ValueString, ValueAsObject, TypeClass, DataTypeInstanceType, ValueStringComplete, DataTypeID, InstanceID)
        //{
        //    this.SubTypes = SubTypes;
        //}
        
        /// <summary>
        /// Gets or Sets SubTypes
        /// </summary>
        [DataMember(Name="subTypes", EmitDefaultValue=false)]
        public List<IJSONTypeInstance> SubTypes { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class IJSONComplexTypeInstance {\n");
            sb.Append("  ").Append(base.ToString().Replace("\n", "\n  ")).Append("\n");
            sb.Append("  SubTypes: ").Append(SubTypes).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public override string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as IJSONComplexTypeInstance);
        }

        /// <summary>
        /// Returns true if IJSONComplexTypeInstance instances are equal
        /// </summary>
        /// <param name="input">Instance of IJSONComplexTypeInstance to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(IJSONComplexTypeInstance input)
        {
            if (input == null)
                return false;

            return base.Equals(input) && 
                (
                    this.SubTypes == input.SubTypes ||
                    this.SubTypes != null &&
                    this.SubTypes.SequenceEqual(input.SubTypes)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = base.GetHashCode();
                if (this.SubTypes != null)
                    hashCode = hashCode * 59 + this.SubTypes.GetHashCode();
                return hashCode;
            }
        }

    }

}