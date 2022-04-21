using System.Collections.Generic;
using BuyerRegistration.Domain.Exceptions;

namespace BuyerRegistration.Api.Domain.Model
{
    public class ValidationFailedResponse
    {
        public List<ValidationError> ValidationResults { get; }

        public ValidationFailedResponse(List<ValidationError> validationResults)
        {
            ValidationResults = validationResults;
        }
    }
}
