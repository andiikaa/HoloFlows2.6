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
    /// IJSONIntegerType
    /// </summary>
    [DataContract]
    public partial class IJSONIntegerType : IJSONType,  IEquatable<IJSONIntegerType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IJSONIntegerType" /> class.
        /// </summary>
        /// <param name="Max">Max.</param>
        /// <param name="Min">Min.</param>
        //public IJSONIntegerType(int? Max = default(int?), int? Min = default(int?), string Name = default(string), string Id = default(string), DataTypeTypeEnum? DataTypeType = default(DataTypeTypeEnum?), string TypeClass = default(string)) : base(Name, Id, DataTypeType, TypeClass)
        //{
        //    this.Max = Max;
        //    this.Min = Min;
        //}
        
        /// <summary>
        /// Gets or Sets Max
        /// </summary>
        [DataMember(Name="max", EmitDefaultValue=false)]
        public int? Max { get; set; }

        /// <summary>
        /// Gets or Sets Min
        /// </summary>
        [DataMember(Name="min", EmitDefaultValue=false)]
        public int? Min { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class IJSONIntegerType {\n");
            sb.Append("  ").Append(base.ToString().Replace("\n", "\n  ")).Append("\n");
            sb.Append("  Max: ").Append(Max).Append("\n");
            sb.Append("  Min: ").Append(Min).Append("\n");
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
            return this.Equals(input as IJSONIntegerType);
        }

        /// <summary>
        /// Returns true if IJSONIntegerType instances are equal
        /// </summary>
        /// <param name="input">Instance of IJSONIntegerType to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(IJSONIntegerType input)
        {
            if (input == null)
                return false;

            return base.Equals(input) && 
                (
                    this.Max == input.Max ||
                    (this.Max != null &&
                    this.Max.Equals(input.Max))
                ) && base.Equals(input) && 
                (
                    this.Min == input.Min ||
                    (this.Min != null &&
                    this.Min.Equals(input.Min))
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
                if (this.Max != null)
                    hashCode = hashCode * 59 + this.Max.GetHashCode();
                if (this.Min != null)
                    hashCode = hashCode * 59 + this.Min.GetHashCode();
                return hashCode;
            }
        }

    }

}