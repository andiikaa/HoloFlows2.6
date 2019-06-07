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
using System.Runtime.Serialization;
using System.Text;

namespace Processes.Proteus.Rest.Model
{
    /// <summary>
    /// IJSONBooleanTypeInstance
    /// </summary>
    [DataContract]
    public partial class IJSONBooleanTypeInstance : IJSONTypeInstance,  IEquatable<IJSONBooleanTypeInstance>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IJSONBooleanTypeInstance" /> class.
        /// </summary>
        /// <param name="Value">Value.</param>
        /// <param name="ValueAsObject">ValueAsObject.</param>
        //public IJSONBooleanTypeInstance(bool? Value, bool? ValueAsObject, string Name, IJSONType DataType, string ValueString, string TypeClass, DataTypeInstanceTypeEnum? DataTypeInstanceType, string ValueStringComplete, string DataTypeID, string InstanceID) : base(Name, DataType, ValueString, TypeClass, DataTypeInstanceType, ValueStringComplete, DataTypeID, InstanceID)
        //{
        //    this.Value = Value;
        //    this.ValueAsObject = ValueAsObject;
        //}
        
        /// <summary>
        /// Gets or Sets Value
        /// </summary>
        [DataMember(Name="value", EmitDefaultValue=false)]
        public bool? Value { get; set; }

        /// <summary>
        /// Gets or Sets ValueAsObject
        /// </summary>
        [DataMember(Name="valueAsObject", EmitDefaultValue=false)]
        new public bool? ValueAsObject { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class IJSONBooleanTypeInstance {\n");
            sb.Append("  ").Append(base.ToString().Replace("\n", "\n  ")).Append("\n");
            sb.Append("  Value: ").Append(Value).Append("\n");
            sb.Append("  ValueAsObject: ").Append(ValueAsObject).Append("\n");
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
            return this.Equals(input as IJSONBooleanTypeInstance);
        }

        /// <summary>
        /// Returns true if IJSONBooleanTypeInstance instances are equal
        /// </summary>
        /// <param name="input">Instance of IJSONBooleanTypeInstance to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(IJSONBooleanTypeInstance input)
        {
            if (input == null)
                return false;

            return base.Equals(input) && 
                (
                    this.Value == input.Value ||
                    (this.Value != null &&
                    this.Value.Equals(input.Value))
                ) && base.Equals(input) && 
                (
                    this.ValueAsObject == input.ValueAsObject ||
                    (this.ValueAsObject != null &&
                    this.ValueAsObject.Equals(input.ValueAsObject))
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
                if (this.Value != null)
                    hashCode = hashCode * 59 + this.Value.GetHashCode();
                if (this.ValueAsObject != null)
                    hashCode = hashCode * 59 + this.ValueAsObject.GetHashCode();
                return hashCode;
            }
        }

    }

}