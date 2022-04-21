using System.Collections.Generic;

namespace BuyerRegistration.Domain.Entities
{
    public class Marketplace
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PlatformId { get; set; }
    }

    public class DestinationPlatform
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Marketplace> Marketplaces { get; set; }
    }
   
}
