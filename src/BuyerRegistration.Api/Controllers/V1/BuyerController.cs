using System.Threading.Tasks;
using BuyerRegistration.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BuyerRegistration.Api.Mappers;
using FluentValidation;
using BuyerRegistration.Api.Domain.Model;

namespace BuyerRegistration.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1")]
    [Route("v1")]
    public partial class BuyerController : ControllerBase
    {
        private readonly IBidderRepository _bidderRepository;
        private readonly IValidator<BidderRequest> _upsertBidderValidator;

        public BuyerController(IBidderRepository bidderRepository, IValidator<BidderRequest> upsertBidderValidator)
        {
            _upsertBidderValidator = upsertBidderValidator;
            _bidderRepository = bidderRepository;
        }

        /// <summary> Put buyer</summary>
        /// <param name="bidderRequest">The buyer request details</param>
        [HttpPut]
        [Route("buyer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationFailedResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PutBidder([FromBody] BidderRequest bidderRequest)
        {
            var validateResult = _upsertBidderValidator.ValidateAsync(bidderRequest); 
            if (validateResult.Result.IsValid)
            {
                BidderData bidderData = BidderDataMapper.MapToBidderData(bidderRequest);
                await _bidderRepository.Persist(bidderData);
                return Ok();
            }

            var validationErrors = ValidationFailedResponseMapper.MapToValidationError(validateResult.Result);

            return BadRequest(new ValidationFailedResponse(validationErrors));
        }
    }
}
