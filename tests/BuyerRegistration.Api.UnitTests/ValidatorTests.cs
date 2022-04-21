using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace BuyerRegistration.Api.UnitTests
{
    using BuyerRegistration.Api.Domain.Model;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using global::BuyerRegistration.Api.Validators;
    using global::BuyerRegistration.Domain;
    using Xunit;

    namespace SLB.CoreBroker.Api.UnitTests.Tests.FluentValidations
    {
        public class ValidatorTests
        {
            private readonly IValidator<BidderRequest> _upsertBidderValidator;

            public ValidatorTests()
            {
                _upsertBidderValidator = new UpsertBidderValidator();
            }          

            [Fact]
            public void CustomerId_When_Characters_GreaterThanFifty_ShouldHave_ValidationError()
            {
                string customerId = new string('a', 51);
                var upsertBidderRequest = CommonUtilities.BidderRequestData(customerId: customerId);
                var result = _upsertBidderValidator.TestValidate(upsertBidderRequest);
                result.IsValid.Should().Be(false);
                CommonAssertions.AssertResultsContainsError(result, ErrorCode.ERROR_INVALID_CUSTOMERID.ToString());
            }

            [Fact]
            public void CustomerId_When_Empty_ShouldHave_ValidationError()
            {
                string customerId = "";
                var upsertBidderRequest = CommonUtilities.BidderRequestData(customerId: customerId);
                var result = _upsertBidderValidator.TestValidate(upsertBidderRequest);
                result.IsValid.Should().Be(false);
                CommonAssertions.AssertResultsContainsError(result, ErrorCode.ERROR_INVALID_CUSTOMERID.ToString());
            }

            [Theory]
            [InlineData("testcustomer#1")]
            [InlineData("testcustomer/1")]
            [InlineData("testcustomer\\1")]
            [InlineData("testcustomer?1")]
            public void CustomerId__IsInvalid_ShouldHave_ValidationError(string customerId)
            {
                var upsertBidderRequest = CommonUtilities.BidderRequestData(customerId: customerId);
                var result = _upsertBidderValidator.TestValidate(upsertBidderRequest);
                result.IsValid.Should().Be(false);
                CommonAssertions.AssertResultsContainsError(result, ErrorCode.ERROR_INVALID_CUSTOMERID.ToString());
            }

            [Fact]
            public void CustomerId__IsValidCharacters_ShouldHave_NoValidationError()
            {
                var customerId = "test!:{}&*()£$%^";
                var upsertBidderRequest = CommonUtilities.BidderRequestData(customerId: customerId);
                var result = _upsertBidderValidator.TestValidate(upsertBidderRequest);
                result.IsValid.Should().Be(true);                
            }




            [Fact]  
            public void BuyerId_When_Characters_GreaterThanFifty_ShouldHave_ValidationError()
            {
                string buyerId = new string('a', 51);
                var upsertBidderRequest = CommonUtilities.BidderRequestData(buyerId: buyerId);
                var result = _upsertBidderValidator.TestValidate(upsertBidderRequest);
                result.IsValid.Should().Be(false);
                CommonAssertions.AssertResultsContainsError(result, ErrorCode.ERROR_INVALID_BUYERID.ToString());
            }

            [Fact]
            public void BuyerId_When_Empty_ShouldHave_ValidationError()
            {
                string buyerId = "";
                var upsertBidderRequest = CommonUtilities.BidderRequestData(buyerId: buyerId);
                var result = _upsertBidderValidator.TestValidate(upsertBidderRequest);
                result.IsValid.Should().Be(false);
                CommonAssertions.AssertResultsContainsError(result, ErrorCode.ERROR_INVALID_BUYERID.ToString());
            }


            [Theory]
            [InlineData("testbidder#1")]
            [InlineData("testbidder/1")]
            [InlineData("testbidder\\1")]
            [InlineData("testbidder?1")]
            public void BuyerId_IsInvalid_ShouldHave_ValidationError(string buyerId)
            {
                var upsertBidderRequest = CommonUtilities.BidderRequestData(buyerId: buyerId);
                var result = _upsertBidderValidator.TestValidate(upsertBidderRequest);
                result.IsValid.Should().Be(false);
                CommonAssertions.AssertResultsContainsError(result, ErrorCode.ERROR_INVALID_BUYERID.ToString());
            }

            [Fact]
            public void BuyerId_IsValidCharacters_ShouldHave_NoValidationError()
            {
                var buyerId = "test!:{}&*()£$%^";
                var upsertBidderRequest = CommonUtilities.BidderRequestData(buyerId: buyerId);
                var result = _upsertBidderValidator.TestValidate(upsertBidderRequest);
                result.IsValid.Should().Be(true);
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(0)]
            public void MarketIdentityCode_When_LessThanOrEqualZero_ShouldHave_ValidationError(int marketIdentityCode)
            {
                var upsertBidderRequest = CommonUtilities.BidderRequestData(marketIdentityCode: marketIdentityCode);
                var result = _upsertBidderValidator.TestValidate(upsertBidderRequest);
                result.IsValid.Should().Be(false);
                CommonAssertions.AssertResultsContainsError(result, ErrorCode.ERROR_INVALID_MARKETIDENTITYCODE.ToString());
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(0)]
            public void TenderHouseId_When_LessThanOrEqualZero_ShouldHave_ValidationError(int tenderHouseId)
            {
                var upsertBidderRequest = CommonUtilities.BidderRequestData(tenderHouseId: tenderHouseId);
                var result = _upsertBidderValidator.TestValidate(upsertBidderRequest);
                result.IsValid.Should().Be(false);
                CommonAssertions.AssertResultsContainsError(result, ErrorCode.ERROR_INVALID_TENDERHOUSEID.ToString());
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(0)]
            public void TenderId_When_LessThanOrEqualZero_ShouldHave_ValidationError(int tenderId)
            {
                var upsertBidderRequest = CommonUtilities.BidderRequestData(tenderId: tenderId);
                var result = _upsertBidderValidator.TestValidate(upsertBidderRequest);
                result.IsValid.Should().Be(false);
                CommonAssertions.AssertResultsContainsError(result, ErrorCode.ERROR_INVALID_TENDERID.ToString());
            }

            [Fact]
            public void CTA_When_Characters_GreaterThanThousand_ShouldHave_ValidationError()
            {
                string cta = new string('a', 1001);
                var upsertBidderRequest = CommonUtilities.BidderRequestData(action: cta);
                var result = _upsertBidderValidator.TestValidate(upsertBidderRequest);
                result.IsValid.Should().Be(false);
                CommonAssertions.AssertResultsContainsError(result, ErrorCode.ERROR_INVALID_ACTION.ToString());
            }

            [Fact]
            public void BuyerRef_When_Characters_GreaterThanFifty_ShouldHave_ValidationError()
            {
                string buyerRef = new string('a', 51);
                var upsertBidderRequest = CommonUtilities.BidderRequestData(buyerRef: buyerRef);
                var result = _upsertBidderValidator.TestValidate(upsertBidderRequest);
                result.IsValid.Should().Be(false);
                CommonAssertions.AssertResultsContainsError(result, ErrorCode.ERROR_INVALID_BUYERREF.ToString());
            }

            [Fact]
            public void BuyerRef_When_Empty_ShouldHave_ValidationError()
            {
                string buyerRef = "";
                var upsertBidderRequest = CommonUtilities.BidderRequestData(buyerRef: buyerRef);
                var result = _upsertBidderValidator.TestValidate(upsertBidderRequest);
                result.IsValid.Should().Be(false);
                CommonAssertions.AssertResultsContainsError(result, ErrorCode.ERROR_INVALID_BUYERREF.ToString());
            }


            [Theory]
            [InlineData(Status.None)]
            public void STATUS_When_NONE_ShouldHave_ValidationError(Status status)
            {
                var upsertBidderRequest = CommonUtilities.BidderRequestData(status: status);
                var result = _upsertBidderValidator.TestValidate(upsertBidderRequest);
                result.IsValid.Should().Be(false);
                CommonAssertions.AssertResultsContainsError(result, ErrorCode.ERROR_INVALID_STATUS.ToString());
            }
        }

        public static class CommonAssertions
        {
            public static bool AssertResultsContainsError<T>(TestValidationResult<T> result, string error)
            {
                var hasCorrectError = result.Errors.Any(x => x.CustomState.ToString() == error);               
                hasCorrectError.Should().Be(true);
                return hasCorrectError;
            }

            public static bool AssertResultsDoesNotContainError<T>(TestValidationResult<T> result, string error)
            {
                var hasError = result.Errors.Any(x => x.CustomState.ToString() == error);
                hasError.Should().Be(false);
                return hasError;
            }
        }

        public class CommonUtilities
        {
            public static BidderRequest BidderRequestData(
                string customerId = "a_customer_id",
                string buyerId = "a_bidder_id",
                int marketIdentityCode = 201,
                int tenderHouseId = 20,
                long tenderId = 30,
                Status status = Status.Approved,
                string buyerRef = "10",
                string action = "test html")
            {
                return new BidderRequest
                {
                    CustomerId = customerId,
                    BuyerId = buyerId,
                    MarketIdentityCode = marketIdentityCode,
                    TenderHouseId = tenderHouseId,
                    TenderId = tenderId,
                    Status = status,
                    BuyerRef = buyerRef,
                    Action = action
                };
            }
        }

    }
}