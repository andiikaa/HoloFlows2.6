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
    /// IJSONSetType
    /// </summary>
    [DataContract]
    public partial class IJSONSetType : IJSONType,  IEquatable<IJSONSetType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IJSONSetType" /> class.
        /// </summary>
        /// <param name="CollectionType">CollectionType.</param>
        public IJSONSetType(IJSONType CollectionType = default(IJSONType), string Name = default(string), string Id = default(string), DataTypeTypeEnum? DataTypeType = default(DataTypeTypeEnum?), string TypeClass = default(string)) : base(Name, Id, DataTypeType, TypeClass)
        {
            this.CollectionType = CollectionType;
        }
        
        /// <summary>
        /// Gets or Sets CollectionType
        /// </summary>
        [DataMember(Name="collectionType", EmitDefaultValue=false)]
        public IJSONType CollectionType { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class IJSONSetType {\n");
            sb.Append("  ").Append(base.ToString().Replace("\n", "\n  ")).Append("\n");
            sb.Append("  CollectionType: ").Append(CollectionType).Append("\n");
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
            return this.Equals(input as IJSONSetType);
        }

        /// <summary>
        /// Returns true if IJSONSetType instances are equal
        /// </summary>
        /// <param name="input">Instance of IJSONSetType to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(IJSONSetType input)
        {
            if (input == null)
                return false;

            return base.Equals(input) && 
                (
                    this.CollectionType == input.CollectionType ||
                    (this.CollectionType != null &&
                    this.CollectionType.Equals(input.CollectionType))
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
                if (this.CollectionType != null)
                    hashCode = hashCode * 59 + this.CollectionType.GetHashCode();
                return hashCode;
            }
        }

    }

}