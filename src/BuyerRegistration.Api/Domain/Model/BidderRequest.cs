using System.ComponentModel.DataAnnotations;
using BuyerRegistration.Domain;

namespace BuyerRegistration.Api.Domain.Model
{
    public class BidderRequest
    {
        [Required]
        public int? MarketIdentityCode { get; set; }
        [Required]
        public long? TenderId { get; set; }
        [Required]
        public int? TenderHouseId { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string BuyerId { get; set; }

        [Required]
        public Status? Status { get; set; }
        [Required]
        public string BuyerRef { get; set; }
        public string? Action { get; set; }
    }
}