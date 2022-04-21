using BuyerRegistration.Api.Domain.Model;
using BuyerRegistration.Domain;
using BuyerRegistration.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BuyerRegistration.Api.Validators
{
    public class UpsertBidderValidator : AbstractValidator<BidderRequest>
    {
        public UpsertBidderValidator()
        {     
            RuleFor(b => b.Action)
               .NotEmpty().When(x => HasPendingOrDeclined(x.Status ?? Status.None))
               .WithCustom(ErrorCode.ERROR_MISSING_DATA).WithMessage("The Action field should not be empty when Status is Pending or Declined");

            RuleFor(b => b.Status).NotEqual(Status.None)
               .WithCustom(ErrorCode.ERROR_INVALID_STATUS).WithMessage("The Status field should not be None");

            RuleFor(b => b.CustomerId)
               .MaximumLength(50).WithCustom(ErrorCode.ERROR_INVALID_CUSTOMERID).WithMessage("The CustomerId field should be less than 50 characters");

            RuleFor(b => b.CustomerId)
               .NotEmpty()
               .WithCustom(ErrorCode.ERROR_INVALID_CUSTOMERID).WithMessage("The CustomerId field should not be empty");

            RuleFor(b => b.CustomerId)
           .Must(BeValidCustomerOrBidder)
           .WithCustom(ErrorCode.ERROR_INVALID_CUSTOMERID).WithMessage("Characters ['/', '\', '?', '#'] cannot be used for CustomerId");

            RuleFor(b => b.BuyerId)
               .MaximumLength(50).WithCustom(ErrorCode.ERROR_INVALID_BUYERID).WithMessage("The BuyerId field should be less than 50 characters");

            RuleFor(b => b.BuyerId)
               .NotEmpty()
               .WithCustom(ErrorCode.ERROR_INVALID_BUYERID).WithMessage("The BuyerId field should not be empty");

          
            RuleFor(b => b.BuyerId)
            .Must(BeValidCustomerOrBidder)
            .WithCustom(ErrorCode.ERROR_INVALID_BUYERID).WithMessage("Characters ['/', '\', '?', '#'] cannot be used for BidderId");


            RuleFor(a => a.TenderId)
             .GreaterThan(0)
             .WithCustom(ErrorCode.ERROR_INVALID_TENDERID).WithMessage("The TenderId field should be greater than zero");

            RuleFor(a => a.TenderHouseId)
           .GreaterThan(0)
           .WithCustom(ErrorCode.ERROR_INVALID_TENDERHOUSEID).WithMessage("The TenderHouseId field should be greater than zero"); 

            RuleFor(a => a.MarketIdentityCode)
            .GreaterThan(0)
            .WithCustom(ErrorCode.ERROR_INVALID_MARKETIDENTITYCODE).WithMessage("The MarketIdentityCode field should be greater than zero");

            RuleFor(a => a.MarketIdentityCode)
                .Must(BeValidMarketPlace)
                .WithCustom(ErrorCode.ERROR_NOTEXISTS_MARKETIDENTITYCODE).WithMessage("MarketIdentityCode does not exist.");

            RuleFor(b => b.BuyerRef)
               .NotEmpty()
               .WithCustom(ErrorCode.ERROR_INVALID_BUYERREF).WithMessage("The BuyerRef field should not be empty");

            RuleFor(b => b.BuyerRef)
               .MaximumLength(50).WithCustom(ErrorCode.ERROR_INVALID_BUYERREF).WithMessage("The BuyerRef field should be less than 50 characters");

            RuleFor(a => a.Action)
             .MaximumLength(1000).WithCustom(ErrorCode.ERROR_INVALID_ACTION).WithMessage("The Action field should be less than 1000 characters");


        }

        private bool BeValidMarketPlace(int? marketPlace)
        {
            if (marketPlace == null)
            {
                return false;
            }

            return MarketplaceValidator.HasValidMarketplaceCode(marketPlace.Value);
        }

        private bool BeValidCustomerOrBidder(string id)
        {
            return CustomerBidderValidator.IsValidCustomerBidder(id);
        }

        public bool HasPendingOrDeclined(Status status)
        {
            return (status == Status.Pending || status == Status.Denied);                
        }
    }

    public enum ErrorCode
    {
        ERROR_MISSING_DATA = 100,  
        ERROR_INVALID_ACTION = 1002,
        ERROR_INVALID_CUSTOMERID = 1003,
        ERROR_INVALID_BUYERID = 1004,
        ERROR_INVALID_TENDERID = 1005,
        ERROR_INVALID_MARKETIDENTITYCODE = 1006,
        ERROR_INVALID_TENDERHOUSEID = 1007,
        ERROR_INVALID_STATUS = 1008,
        ERROR_INVALID_BUYERREF = 1009,
        ERROR_NOTEXISTS_MARKETIDENTITYCODE = 1010,
    }

    public static class MarketplaceValidator
    {
        private static readonly List<DestinationPlatform> DestinationPlatform;

        static MarketplaceValidator()
        {
            DestinationPlatform = new List<DestinationPlatform>();
            DestinationPlatform = JsonSerializer.Deserialize<List<DestinationPlatform>>(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "MarketPlaceLookup.json")), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, });

        }
        public static bool HasValidMarketplaceCode(int marketplaceCode)
        {
            return DestinationPlatform.Any(x => x.Marketplaces.Any( y=> y.Id == marketplaceCode));
        }
    }


    public static class CustomerBidderValidator
    {
        private const string CustomerBidderValidatorRegEx = @"[#?/\\]";

        /// <summary>
        /// Checks whether the email id is in the valid format or not.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidCustomerBidder(string id)
        {
            var regex = new Regex(CustomerBidderValidatorRegEx);
            return !regex.IsMatch(id);
        }
    }
}
