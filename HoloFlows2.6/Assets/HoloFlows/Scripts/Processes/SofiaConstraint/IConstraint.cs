using HoloFlows.Processes.Sofia;
using HoloFlows.Processes.SofiaUtil;
using System;

namespace HoloFlows.Processes.SofiaConstraint
{
    public interface IConstraint
    {
        Type TargetType { get; }
        bool CanValidateObject(object obj);
        ModelValidationError Validate(ValidationContext context);
    }

    /// <summary>
    /// Checks if each id is unique and not null or empty in the model.
    /// </summary>
    public class UniqueIDConstraint : IConstraint
    {
        public Type TargetType { get { return typeof(Identifiable); } }

        public bool CanValidateObject(object obj) { return obj is Identifiable; }

        public ModelValidationError Validate(ValidationContext context)
        {
            var target = context.Target as Identifiable;
            bool isMissingId = string.IsNullOrEmpty(target.id);
            if (isMissingId) return new ModelValidationError() { ErrorMessage = "Id is missing for some " + target.GetType().Name };

            if (context.ConstraintData.ContainsKey(target.id))
                return new ModelValidationError() { ErrorMessage = "Id is not unique: " + target.id };
            context.ConstraintData.Add(target.id, target);
            return null;
        }
    }
}
