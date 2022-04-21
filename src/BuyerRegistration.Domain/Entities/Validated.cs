using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuyerRegistration.Domain.Exceptions;

namespace BuyerRegistration.Domain.Entities
{
    public class CustomValidationResult
    {
        public bool IsValid { get; set; }
        public List<ValidationError> ValidationResults { get; } = new();

        public List<TransformationError> TransformationResults { get; set; } = new();

        public CustomValidationResult()
        {

        }

        public CustomValidationResult(bool isValid, List<ValidationError> validationResults)
        {
            IsValid = isValid;
            ValidationResults = validationResults;
        }

        public CustomValidationResult(bool isValid, List<ValidationError> validationResults, List<TransformationError> transformationResults)
        {
            IsValid = isValid;
            ValidationResults = validationResults;
            TransformationResults = transformationResults;
        }

        public CustomValidationResult(List<ValidationError> validationResults)
        {
            ValidationResults = validationResults;
            IsValid = !validationResults.Any();
        }

        public void AddValidationResults(List<ValidationError> validationErrors)
        {
            ValidationResults.AddRange(validationErrors);
            IsValid &= !validationErrors.Any();
        }

        public void AddTransformationErrors(List<TransformationError> transformationErrors)
        {
            ValidationResults.AddRange(transformationErrors);
        }

        public void AddValidationResults(CustomValidationResult validationResult)
        {
            ValidationResults.AddRange(validationResult.ValidationResults);
            IsValid &= validationResult.IsValid;
        }
    }

    public class Validated<T> : CustomValidationResult where T : class
    {
        public T Value { get; set; }
        public Validated(T value, bool isValid, List<ValidationError> validationResults) : base(isValid, validationResults)
        {
            Value = value;
        }

        public Validated(T value, bool isValid, List<ValidationError> validationResults, List<TransformationError> transformationResults) : base(isValid, validationResults, transformationResults)
        {
            Value = value;
        }


        public Validated(bool isValid, List<ValidationError> validationResults) : base(isValid, validationResults)
        {
        }

        public Validated(List<ValidationError> validationResults) : base(validationResults)
        {
        }

        public Validated(CustomValidationResult validationResult) : base(validationResult.IsValid, validationResult.ValidationResults)
        {

        }
    }
}
