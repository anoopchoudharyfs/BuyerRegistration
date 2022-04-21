namespace BuyerRegistration.Domain
{
    public class BidderData
    {
        public string Id { get; set; }
        public string PartitionKey { get; set; }
        
        public int? MarketIdentityCode { get; set; }
        
        public long? TenderId { get; set; }
        
        public int? TenderHouseId { get; set; }
        public string CustomerId { get; set; }
        public string BuyerId { get; set; }
        public string BuyerRef { get; set; }
        public Status? Status { get; set; }
        public string? Action { get; set; }
    }
}