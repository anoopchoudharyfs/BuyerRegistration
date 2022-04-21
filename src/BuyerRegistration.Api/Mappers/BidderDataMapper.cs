using BuyerRegistration.Api.Domain.Model;
using BuyerRegistration.Domain;


namespace BuyerRegistration.Api.Mappers
{  
     public static class BidderDataMapper
     {
        public static BidderData MapToBidderData(BidderRequest bidderRequest)
        {
            return new BidderData
            {
                Id = $"{bidderRequest.TenderId}-{bidderRequest.CustomerId}-{bidderRequest.MarketIdentityCode}",
                PartitionKey = $"{bidderRequest.CustomerId}-{bidderRequest.MarketIdentityCode}",
                CustomerId = bidderRequest.CustomerId,
                TenderHouseId = bidderRequest.TenderHouseId,
                TenderId = bidderRequest.TenderId,
                MarketIdentityCode = bidderRequest.MarketIdentityCode,
                BuyerId = bidderRequest.BuyerId,
                BuyerRef = bidderRequest.BuyerRef,
                Status = bidderRequest.Status,
                Action = bidderRequest.Action
            };
        }
     }   
}
