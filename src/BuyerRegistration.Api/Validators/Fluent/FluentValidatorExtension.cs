using FluentValidation;

namespace BuyerRegistration.Api.Validators
{ 
    public static class FluentValidatorExtension
    {
        public static IRuleBuilderOptions<T, TProperty> WithCustom<T, E, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule, E errorCode) where E : System.Enum
        {
            return rule.WithErrorCode((System.Convert.ToInt32(errorCode)).ToString())
                .WithState(x => errorCode.ToString());
        }
    }
}
