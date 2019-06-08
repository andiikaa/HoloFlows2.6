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
    /// IJSONPortInstance
    /// </summary>
    [DataContract]
    [KnownType(typeof(IJSONDataPortInstance))]
    [KnownType(typeof(IJSONEscalationPortInstance))]
    public partial class IJSONPortInstance : IEquatable<IJSONPortInstance>
    {
        protected virtual string CreateJavaClassInfo() { return "eu.vicci.process.model.util.serialization.jsonprocessstepinstances.JSONPortInstance"; }

        [JsonProperty("@class")]
        public string ClassInfo { get { return CreateJavaClassInfo(); } }

        /// <summary>
        /// Gets or Sets PortInstanceType
        /// </summary>
        [DataMember(Name = "portInstanceType", EmitDefaultValue = false)]
        public PortInstanceTypeEnum? PortInstanceType { get; set; }


        /// <summary>
        /// Gets or Sets ExecutionState
        /// </summary>
        [DataMember(Name = "executionState", EmitDefaultValue = false)]
        public ExecutionStateEnum? ExecutionState { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="IJSONPortInstance" /> class.
        /// </summary>
        /// <param name="Name">Name.</param>
        /// <param name="Type">Type.</param>
        /// <param name="OutTransitions">OutTransitions.</param>
        /// <param name="DoDeactivation">DoDeactivation.</param>
        /// <param name="PortInstanceType">PortInstanceType.</param>
        /// <param name="InstanceNumber">InstanceNumber.</param>
        /// <param name="ExecutionState">ExecutionState.</param>
        /// <param name="InstanceId">InstanceId.</param>
        /// <param name="InTransitions">InTransitions.</param>
        /// <param name="TypeId">TypeId.</param>
        /// <param name="PortType">PortType.</param>
        public IJSONPortInstance(string Name = default(string), string Type = default(string), List<IJSONTransitionInstance> OutTransitions = default(List<IJSONTransitionInstance>), bool? DoDeactivation = default(bool?), PortInstanceTypeEnum? PortInstanceType = default(PortInstanceTypeEnum?), int? InstanceNumber = default(int?), ExecutionStateEnum? ExecutionState = default(ExecutionStateEnum?), string InstanceId = default(string), IJSONTransitionInstance InTransitions = default(IJSONTransitionInstance), string TypeId = default(string), IJSONPort PortType = default(IJSONPort))
        {
            this.Name = Name;
            this.Type = Type;
            this.OutTransitions = OutTransitions;
            this.DoDeactivation = DoDeactivation;
            this.PortInstanceType = PortInstanceType;
            this.InstanceNumber = InstanceNumber;
            this.ExecutionState = ExecutionState;
            this.InstanceId = InstanceId;
            this.InTransitions = InTransitions;
            this.TypeId = TypeId;
            this.PortType = PortType;
        }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets OutTransitions
        /// </summary>
        [DataMember(Name = "outTransitions", EmitDefaultValue = false)]
        public List<IJSONTransitionInstance> OutTransitions { get; set; }

        /// <summary>
        /// Gets or Sets DoDeactivation
        /// </summary>
        [DataMember(Name = "doDeactivation", EmitDefaultValue = false)]
        public bool? DoDeactivation { get; set; }


        /// <summary>
        /// Gets or Sets InstanceNumber
        /// </summary>
        [DataMember(Name = "instanceNumber", EmitDefaultValue = false)]
        public int? InstanceNumber { get; set; }


        /// <summary>
        /// Gets or Sets InstanceId
        /// </summary>
        [DataMember(Name = "instanceId", EmitDefaultValue = false)]
        public string InstanceId { get; set; }

        /// <summary>
        /// Gets or Sets InTransitions
        /// </summary>
        [DataMember(Name = "inTransitions", EmitDefaultValue = false)]
        public IJSONTransitionInstance InTransitions { get; set; }

        /// <summary>
        /// Gets or Sets TypeId
        /// </summary>
        [DataMember(Name = "typeId", EmitDefaultValue = false)]
        public string TypeId { get; set; }

        /// <summary>
        /// Gets or Sets PortType
        /// </summary>
        [DataMember(Name = "portType", EmitDefaultValue = false)]
        public IJSONPort PortType { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class IJSONPortInstance {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  OutTransitions: ").Append(OutTransitions).Append("\n");
            sb.Append("  DoDeactivation: ").Append(DoDeactivation).Append("\n");
            sb.Append("  PortInstanceType: ").Append(PortInstanceType).Append("\n");
            sb.Append("  InstanceNumber: ").Append(InstanceNumber).Append("\n");
            sb.Append("  ExecutionState: ").Append(ExecutionState).Append("\n");
            sb.Append("  InstanceId: ").Append(InstanceId).Append("\n");
            sb.Append("  InTransitions: ").Append(InTransitions).Append("\n");
            sb.Append("  TypeId: ").Append(TypeId).Append("\n");
            sb.Append("  PortType: ").Append(PortType).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
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
            return this.Equals(input as IJSONPortInstance);
        }

        /// <summary>
        /// Returns true if IJSONPortInstance instances are equal
        /// </summary>
        /// <param name="input">Instance of IJSONPortInstance to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(IJSONPortInstance input)
        {
            if (input == null)
                return false;

            return
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) &&
                (
                    this.Type == input.Type ||
                    (this.Type != null &&
                    this.Type.Equals(input.Type))
                ) &&
                (
                    this.OutTransitions == input.OutTransitions ||
                    this.OutTransitions != null &&
                    this.OutTransitions.SequenceEqual(input.OutTransitions)
                ) &&
                (
                    this.DoDeactivation == input.DoDeactivation ||
                    (this.DoDeactivation != null &&
                    this.DoDeactivation.Equals(input.DoDeactivation))
                ) &&
                (
                    this.PortInstanceType == input.PortInstanceType ||
                    (this.PortInstanceType != null &&
                    this.PortInstanceType.Equals(input.PortInstanceType))
                ) &&
                (
                    this.InstanceNumber == input.InstanceNumber ||
                    (this.InstanceNumber != null &&
                    this.InstanceNumber.Equals(input.InstanceNumber))
                ) &&
                (
                    this.ExecutionState == input.ExecutionState ||
                    (this.ExecutionState != null &&
                    this.ExecutionState.Equals(input.ExecutionState))
                ) &&
                (
                    this.InstanceId == input.InstanceId ||
                    (this.InstanceId != null &&
                    this.InstanceId.Equals(input.InstanceId))
                ) &&
                (
                    this.InTransitions == input.InTransitions ||
                    (this.InTransitions != null &&
                    this.InTransitions.Equals(input.InTransitions))
                ) &&
                (
                    this.TypeId == input.TypeId ||
                    (this.TypeId != null &&
                    this.TypeId.Equals(input.TypeId))
                ) &&
                (
                    this.PortType == input.PortType ||
                    (this.PortType != null &&
                    this.PortType.Equals(input.PortType))
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
                int hashCode = 41;
                if (this.Name != null)
                    hashCode = hashCode * 59 + this.Name.GetHashCode();
                if (this.Type != null)
                    hashCode = hashCode * 59 + this.Type.GetHashCode();
                if (this.OutTransitions != null)
                    hashCode = hashCode * 59 + this.OutTransitions.GetHashCode();
                if (this.DoDeactivation != null)
                    hashCode = hashCode * 59 + this.DoDeactivation.GetHashCode();
                if (this.PortInstanceType != null)
                    hashCode = hashCode * 59 + this.PortInstanceType.GetHashCode();
                if (this.InstanceNumber != null)
                    hashCode = hashCode * 59 + this.InstanceNumber.GetHashCode();
                if (this.ExecutionState != null)
                    hashCode = hashCode * 59 + this.ExecutionState.GetHashCode();
                if (this.InstanceId != null)
                    hashCode = hashCode * 59 + this.InstanceId.GetHashCode();
                if (this.InTransitions != null)
                    hashCode = hashCode * 59 + this.InTransitions.GetHashCode();
                if (this.TypeId != null)
                    hashCode = hashCode * 59 + this.TypeId.GetHashCode();
                if (this.PortType != null)
                    hashCode = hashCode * 59 + this.PortType.GetHashCode();
                return hashCode;
            }
        }

    }

}
