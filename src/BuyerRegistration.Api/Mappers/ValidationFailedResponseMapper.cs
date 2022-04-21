using System.Collections.Generic;
using BuyerRegistration.Domain.Exceptions;
using System;
using System.Linq;
using FluentValidation.Results;

namespace BuyerRegistration.Api.Mappers
{
    public static class ValidationFailedResponseMapper
    {
        public static List<ValidationError> MapToValidationError(ValidationResult validateResult)
        {
            return validateResult.Errors.Select(x => new ValidationError(Convert.ToInt32(x.ErrorCode), x.CustomState == null ? x.PropertyName : x.CustomState.ToString(), x.ErrorMessage, x.PropertyName))
        .ToList();
        }
    }
}
