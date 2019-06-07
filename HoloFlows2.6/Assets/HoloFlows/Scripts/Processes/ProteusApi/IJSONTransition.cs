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
    /// IJSONTransition
    /// </summary>
    [DataContract]
    public partial class IJSONTransition :  IEquatable<IJSONTransition>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IJSONTransition" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        public IJSONTransition()
        {
        }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class IJSONTransition {\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
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
            return this.Equals(input as IJSONTransition);
        }

        /// <summary>
        /// Returns true if IJSONTransition instances are equal
        /// </summary>
        /// <param name="input">Instance of IJSONTransition to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(IJSONTransition input)
        {
            if (input == null)
                return false;

            return false;
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                return hashCode;
            }
        }

    }

}