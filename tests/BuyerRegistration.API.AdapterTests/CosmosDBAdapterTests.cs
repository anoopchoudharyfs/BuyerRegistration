using BuyerRegistration.Domain;
using BuyerRegistration.Services.Config;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;
using Xunit;
using Microsoft.ApplicationInsights.Extensibility;

namespace BuyerRegistration.API.AdapterTests
{
    public class CosmosDBAdapterTests
    {
        private CosmosDbSettings _cosmosDbSettings = new CosmosDbSettings()
        {
            ContainerName = "buyerdata",
            Database = "buyer-registration",
            ConnectionString = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
           
        };

        private CosmosClient _cosmosClient;

        [Fact]
        public async Task SaveAndReadFromCosmosDB()
        {
            var repo = new BidderRepository(_cosmosDbSettings, new TelemetryClient(TelemetryConfiguration.CreateDefault()));

            var toCreate = new BidderData()
            {
                Id = "1",
                PartitionKey = "10-4",
                MarketIdentityCode = 10,
                TenderId = 2,
                TenderHouseId = 3,
                CustomerId = "4",
                BuyerId = "5",
                Status = Status.Pending,
                Action = "This is a test document created by the adapter test.",
            };

            await repo.Persist(toCreate);

            var cosmosClientOptions = new CosmosClientOptions() { SerializerOptions = new CosmosSerializationOptions() { PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase } };
           
            _cosmosClient = new CosmosClient(_cosmosDbSettings.ConnectionString, cosmosClientOptions);            

            var databaseId = _cosmosDbSettings.Database;

            var container = _cosmosClient.GetContainer(databaseId, _cosmosDbSettings.ContainerName);

            var readResult = await container.ReadItemAsync<BidderData>(toCreate.Id, new PartitionKey(toCreate.PartitionKey));

            Assert.Equal(toCreate.Id, readResult.Resource.Id);
            Assert.Equal(toCreate.PartitionKey, readResult.Resource.PartitionKey);
            Assert.Equal(toCreate.MarketIdentityCode, readResult.Resource.MarketIdentityCode);
            Assert.Equal(toCreate.TenderId, readResult.Resource.TenderId);
            Assert.Equal(toCreate.TenderHouseId, readResult.Resource.TenderHouseId);
            Assert.Equal(toCreate.CustomerId, readResult.Resource.CustomerId);
            Assert.Equal(toCreate.BuyerId, readResult.Resource.BuyerId);
            Assert.Equal(toCreate.Status, readResult.Resource.Status);
            Assert.Equal(toCreate.Action, readResult.Resource.Action);

            await container.DeleteItemAsync<BidderData>(toCreate.Id, new PartitionKey(toCreate.PartitionKey));
        }
    }
}
