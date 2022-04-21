using System.Collections.Generic;

namespace BuyerRegistration.Domain.Exceptions
{
    public class ModelBindingValidationError
    {
        public IList<ValidationError> ValidationResults { get; set; }
    }
}
