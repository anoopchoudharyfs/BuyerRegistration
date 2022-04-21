using System;
using System.Linq;
using System.Threading.Tasks;
using BuyerRegistration.Domain;
using BuyerRegistration.Domain.Entities;
using BuyerRegistration.Domain.Exceptions;
using FluentValidation;

namespace BuyerRegistration.Api.Validators
{
    //public interface IValidateBidder
    //{
    //    Task<CustomValidationResult> Validate(BidderRequest upsertbidderRequest);
    //}

    //public class ValidateBidder : IValidateBidder
    //{
    //    private readonly IValidator<BidderRequest> _upsertBidderValidator;
    //    public ValidateBidder(IValidator<BidderRequest> upsertBidderValidator)
    //    {
    //        _upsertBidderValidator = upsertBidderValidator;
    //    }

    //    public async Task<CustomValidationResult> Validate(BidderRequest upsertBidderRequest)
    //    {
    //        var validationResult = await _upsertBidderValidator.ValidateAsync(upsertBidderRequest);

    //        var validationResults = validationResult.Errors.Select(x => new ValidationError(Convert.ToInt32(x.ErrorCode), x.CustomState == null ? x.PropertyName : x.CustomState.ToString(), x.ErrorMessage, x.PropertyName))
    //            .ToList();

    //        return new CustomValidationResult(validationResult.IsValid, validationResults);
    //    }
    //}
}
