using HoloFlows.Processes.Sofia;
using HoloFlows.Processes.SofiaConstraint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HoloFlows.Processes.SofiaUtil
{
    /// <summary>
    /// Validator which helps to find common model mistakes. 
    /// This should prevent errors on the engine side and should make finding errors more easy.
    /// 
    /// Make sure all model files are in the right (and same) namespace.
    /// This class is not threadsafe!
    /// </summary>
    public class Validator
    {
        private readonly IList<IConstraint> constraints;
        private readonly ISet<object> objectSet;
        private readonly ValidationResult result;
        //we check the namespace, so we have to validate only model types and no string, long, ...
        private readonly string NAMESPACE = "Processes.Sofia";

        public Validator()
        {
            constraints = GetConstraints();
            objectSet = new HashSet<object>();
            result = new ValidationResult();
        }

        /// <summary>
        /// Add new Constraints in this method, to add them to the validation.
        /// </summary>
        /// <returns></returns>
        protected static IList<IConstraint> GetConstraints()
        {
            return new List<IConstraint>
            {
                new UniqueIDConstraint()
            };
        }


        public ValidationResult Validate(Process process)
        {
            if (!IsFromCorrectNamespace(process))
                throw new ArgumentException(string.Format("You are validating an object from the wrong namespace: expected NS: '{0}' actual NS: '{1}'",
                    NAMESPACE, process.GetType().Namespace));
            result.Clear();
            objectSet.Clear();

            objectSet.Add(process);
            AddPropertiesToObjectSet(process);

            ValidateAll();

            return result;
        }

        private void ValidateAll()
        {
            foreach (IConstraint constraint in constraints)
            {
                ValidationContext constraintContext = new ValidationContext();
                foreach (object obj in objectSet)
                {
                    constraintContext.Target = obj;
                    if (constraint.CanValidateObject(obj))
                    {
                        ModelValidationError err = constraint.Validate(constraintContext);
                        if (err != null)
                            result.Errors.Add(err);
                    }
                }
            }
        }


        private void AddPropertiesToObjectSet(object obj)
        {
            foreach (PropertyInfo info in obj.GetType().GetProperties())
            {
                var propValue = info.GetValue(obj);
                if (propValue == null)
                    continue;
                var propList = propValue as IEnumerable;
                if (propList != null)
                {
                    foreach (object to in propList)
                    {
                        AddObjectToObjectSet(to);
                    }
                }
                else
                {
                    AddObjectToObjectSet(propValue);
                }
            }
        }

        private void AddObjectToObjectSet(object obj)
        {
            if (!IsFromCorrectNamespace(obj)) return;
            if (!objectSet.Add(obj)) return;
            AddPropertiesToObjectSet(obj);
        }

        private bool IsFromCorrectNamespace(object obj) { return obj.GetType().Namespace == NAMESPACE; }

    }

    public class ValidationContext
    {
        public ValidationContext()
        {
            ConstraintData = new Dictionary<string, object>();
        }

        public object Target { get; set; }
        public IDictionary<string, object> ConstraintData { get; set; }
    }

    public class ValidationResult
    {
        public ValidationResult()
        {
            Errors = new List<ModelValidationError>();
        }

        public IList<ModelValidationError> Errors { get; set; }
        public bool HasErrors { get { return Errors.Any(); } }

        public void Clear()
        {
            Errors.Clear();
        }
    }

    public class ModelValidationError
    {
        public string ErrorMessage { get; set; }
    }
}
