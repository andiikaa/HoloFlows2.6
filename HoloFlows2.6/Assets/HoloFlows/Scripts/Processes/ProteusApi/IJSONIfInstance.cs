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
    /// IJSONIfInstance
    /// </summary>
    [DataContract]
    public partial class IJSONIfInstance :  IEquatable<IJSONIfInstance>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IJSONIfInstance" /> class.
        /// </summary>
        /// <param name="State">State.</param>
        /// <param name="Type">Type.</param>
        /// <param name="Permission">Permission.</param>
        /// <param name="Delay">Delay.</param>
        /// <param name="InstanceNumber">InstanceNumber.</param>
        /// <param name="ProcessInstanceId">ProcessInstanceId.</param>
        /// <param name="InstanceId">InstanceId.</param>
        /// <param name="Ports">Ports.</param>
        /// <param name="SubSteps">SubSteps.</param>
        /// <param name="ProcessModelId">ProcessModelId.</param>
        /// <param name="SimpleTypeName">SimpleTypeName.</param>
        /// <param name="ModelId">ModelId.</param>
        /// <param name="Comparator">Comparator.</param>
        /// <param name="LeftSide">LeftSide.</param>
        /// <param name="RightSide">RightSide.</param>
        /// <param name="ExpressionInstance">ExpressionInstance.</param>
        /// <param name="Result">Result.</param>
        //public IJSONIfInstance(int? State = default(int?), string Type = default(string), int? Permission = default(int?), long? Delay = default(long?), int? InstanceNumber = default(int?), string ProcessInstanceId = default(string), string InstanceId = default(string), List<IJSONPortInstance> Ports = default(List<IJSONPortInstance>), List<IJSONProcessStepInstance> SubSteps = default(List<IJSONProcessStepInstance>), string ProcessModelId = default(string), string SimpleTypeName = default(string), string ModelId = default(string), int? Comparator = default(int?), IJSONPortInstance LeftSide = default(IJSONPortInstance), IJSONPortInstance RightSide = default(IJSONPortInstance), string ExpressionInstance = default(string), bool? Result = default(bool?))
        //{
        //    this.State = State;
        //    this.Type = Type;
        //    this.Permission = Permission;
        //    this.Delay = Delay;
        //    this.InstanceNumber = InstanceNumber;
        //    this.ProcessInstanceId = ProcessInstanceId;
        //    this.InstanceId = InstanceId;
        //    this.Ports = Ports;
        //    this.SubSteps = SubSteps;
        //    this.ProcessModelId = ProcessModelId;
        //    this.SimpleTypeName = SimpleTypeName;
        //    this.ModelId = ModelId;
        //    this.Comparator = Comparator;
        //    this.LeftSide = LeftSide;
        //    this.RightSide = RightSide;
        //    this.ExpressionInstance = ExpressionInstance;
        //    this.Result = Result;
        //}
        
        /// <summary>
        /// Gets or Sets State
        /// </summary>
        [DataMember(Name="state", EmitDefaultValue=false)]
        public int? State { get; set; }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name="type", EmitDefaultValue=false)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets Permission
        /// </summary>
        [DataMember(Name="permission", EmitDefaultValue=false)]
        public int? Permission { get; set; }

        /// <summary>
        /// Gets or Sets Delay
        /// </summary>
        [DataMember(Name="delay", EmitDefaultValue=false)]
        public long? Delay { get; set; }

        /// <summary>
        /// Gets or Sets InstanceNumber
        /// </summary>
        [DataMember(Name="instanceNumber", EmitDefaultValue=false)]
        public int? InstanceNumber { get; set; }

        /// <summary>
        /// Gets or Sets ProcessInstanceId
        /// </summary>
        [DataMember(Name="processInstanceId", EmitDefaultValue=false)]
        public string ProcessInstanceId { get; set; }

        /// <summary>
        /// Gets or Sets InstanceId
        /// </summary>
        [DataMember(Name="instanceId", EmitDefaultValue=false)]
        public string InstanceId { get; set; }

        /// <summary>
        /// Gets or Sets Ports
        /// </summary>
        [DataMember(Name="ports", EmitDefaultValue=false)]
        public List<IJSONPortInstance> Ports { get; set; }

        /// <summary>
        /// Gets or Sets SubSteps
        /// </summary>
        [DataMember(Name="subSteps", EmitDefaultValue=false)]
        public List<IJSONProcessStepInstance> SubSteps { get; set; }

        /// <summary>
        /// Gets or Sets ProcessModelId
        /// </summary>
        [DataMember(Name="processModelId", EmitDefaultValue=false)]
        public string ProcessModelId { get; set; }

        /// <summary>
        /// Gets or Sets SimpleTypeName
        /// </summary>
        [DataMember(Name="simpleTypeName", EmitDefaultValue=false)]
        public string SimpleTypeName { get; set; }

        /// <summary>
        /// Gets or Sets ModelId
        /// </summary>
        [DataMember(Name="modelId", EmitDefaultValue=false)]
        public string ModelId { get; set; }

        /// <summary>
        /// Gets or Sets Comparator
        /// </summary>
        [DataMember(Name="comparator", EmitDefaultValue=false)]
        public int? Comparator { get; set; }

        /// <summary>
        /// Gets or Sets LeftSide
        /// </summary>
        [DataMember(Name="leftSide", EmitDefaultValue=false)]
        public IJSONPortInstance LeftSide { get; set; }

        /// <summary>
        /// Gets or Sets RightSide
        /// </summary>
        [DataMember(Name="rightSide", EmitDefaultValue=false)]
        public IJSONPortInstance RightSide { get; set; }

        /// <summary>
        /// Gets or Sets ExpressionInstance
        /// </summary>
        [DataMember(Name="expressionInstance", EmitDefaultValue=false)]
        public string ExpressionInstance { get; set; }

        /// <summary>
        /// Gets or Sets Result
        /// </summary>
        [DataMember(Name="result", EmitDefaultValue=false)]
        public bool? Result { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class IJSONIfInstance {\n");
            sb.Append("  State: ").Append(State).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Permission: ").Append(Permission).Append("\n");
            sb.Append("  Delay: ").Append(Delay).Append("\n");
            sb.Append("  InstanceNumber: ").Append(InstanceNumber).Append("\n");
            sb.Append("  ProcessInstanceId: ").Append(ProcessInstanceId).Append("\n");
            sb.Append("  InstanceId: ").Append(InstanceId).Append("\n");
            sb.Append("  Ports: ").Append(Ports).Append("\n");
            sb.Append("  SubSteps: ").Append(SubSteps).Append("\n");
            sb.Append("  ProcessModelId: ").Append(ProcessModelId).Append("\n");
            sb.Append("  SimpleTypeName: ").Append(SimpleTypeName).Append("\n");
            sb.Append("  ModelId: ").Append(ModelId).Append("\n");
            sb.Append("  Comparator: ").Append(Comparator).Append("\n");
            sb.Append("  LeftSide: ").Append(LeftSide).Append("\n");
            sb.Append("  RightSide: ").Append(RightSide).Append("\n");
            sb.Append("  ExpressionInstance: ").Append(ExpressionInstance).Append("\n");
            sb.Append("  Result: ").Append(Result).Append("\n");
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
            return this.Equals(input as IJSONIfInstance);
        }

        /// <summary>
        /// Returns true if IJSONIfInstance instances are equal
        /// </summary>
        /// <param name="input">Instance of IJSONIfInstance to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(IJSONIfInstance input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.State == input.State ||
                    (this.State != null &&
                    this.State.Equals(input.State))
                ) && 
                (
                    this.Type == input.Type ||
                    (this.Type != null &&
                    this.Type.Equals(input.Type))
                ) && 
                (
                    this.Permission == input.Permission ||
                    (this.Permission != null &&
                    this.Permission.Equals(input.Permission))
                ) && 
                (
                    this.Delay == input.Delay ||
                    (this.Delay != null &&
                    this.Delay.Equals(input.Delay))
                ) && 
                (
                    this.InstanceNumber == input.InstanceNumber ||
                    (this.InstanceNumber != null &&
                    this.InstanceNumber.Equals(input.InstanceNumber))
                ) && 
                (
                    this.ProcessInstanceId == input.ProcessInstanceId ||
                    (this.ProcessInstanceId != null &&
                    this.ProcessInstanceId.Equals(input.ProcessInstanceId))
                ) && 
                (
                    this.InstanceId == input.InstanceId ||
                    (this.InstanceId != null &&
                    this.InstanceId.Equals(input.InstanceId))
                ) && 
                (
                    this.Ports == input.Ports ||
                    this.Ports != null &&
                    this.Ports.SequenceEqual(input.Ports)
                ) && 
                (
                    this.SubSteps == input.SubSteps ||
                    this.SubSteps != null &&
                    this.SubSteps.SequenceEqual(input.SubSteps)
                ) && 
                (
                    this.ProcessModelId == input.ProcessModelId ||
                    (this.ProcessModelId != null &&
                    this.ProcessModelId.Equals(input.ProcessModelId))
                ) && 
                (
                    this.SimpleTypeName == input.SimpleTypeName ||
                    (this.SimpleTypeName != null &&
                    this.SimpleTypeName.Equals(input.SimpleTypeName))
                ) && 
                (
                    this.ModelId == input.ModelId ||
                    (this.ModelId != null &&
                    this.ModelId.Equals(input.ModelId))
                ) && 
                (
                    this.Comparator == input.Comparator ||
                    (this.Comparator != null &&
                    this.Comparator.Equals(input.Comparator))
                ) && 
                (
                    this.LeftSide == input.LeftSide ||
                    (this.LeftSide != null &&
                    this.LeftSide.Equals(input.LeftSide))
                ) && 
                (
                    this.RightSide == input.RightSide ||
                    (this.RightSide != null &&
                    this.RightSide.Equals(input.RightSide))
                ) && 
                (
                    this.ExpressionInstance == input.ExpressionInstance ||
                    (this.ExpressionInstance != null &&
                    this.ExpressionInstance.Equals(input.ExpressionInstance))
                ) && 
                (
                    this.Result == input.Result ||
                    (this.Result != null &&
                    this.Result.Equals(input.Result))
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
                if (this.State != null)
                    hashCode = hashCode * 59 + this.State.GetHashCode();
                if (this.Type != null)
                    hashCode = hashCode * 59 + this.Type.GetHashCode();
                if (this.Permission != null)
                    hashCode = hashCode * 59 + this.Permission.GetHashCode();
                if (this.Delay != null)
                    hashCode = hashCode * 59 + this.Delay.GetHashCode();
                if (this.InstanceNumber != null)
                    hashCode = hashCode * 59 + this.InstanceNumber.GetHashCode();
                if (this.ProcessInstanceId != null)
                    hashCode = hashCode * 59 + this.ProcessInstanceId.GetHashCode();
                if (this.InstanceId != null)
                    hashCode = hashCode * 59 + this.InstanceId.GetHashCode();
                if (this.Ports != null)
                    hashCode = hashCode * 59 + this.Ports.GetHashCode();
                if (this.SubSteps != null)
                    hashCode = hashCode * 59 + this.SubSteps.GetHashCode();
                if (this.ProcessModelId != null)
                    hashCode = hashCode * 59 + this.ProcessModelId.GetHashCode();
                if (this.SimpleTypeName != null)
                    hashCode = hashCode * 59 + this.SimpleTypeName.GetHashCode();
                if (this.ModelId != null)
                    hashCode = hashCode * 59 + this.ModelId.GetHashCode();
                if (this.Comparator != null)
                    hashCode = hashCode * 59 + this.Comparator.GetHashCode();
                if (this.LeftSide != null)
                    hashCode = hashCode * 59 + this.LeftSide.GetHashCode();
                if (this.RightSide != null)
                    hashCode = hashCode * 59 + this.RightSide.GetHashCode();
                if (this.ExpressionInstance != null)
                    hashCode = hashCode * 59 + this.ExpressionInstance.GetHashCode();
                if (this.Result != null)
                    hashCode = hashCode * 59 + this.Result.GetHashCode();
                return hashCode;
            }
        }

    }

}
