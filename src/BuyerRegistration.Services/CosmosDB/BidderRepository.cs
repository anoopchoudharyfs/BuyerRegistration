using BuyerRegistration.Services.Config;
using Azure.Identity;
using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace BuyerRegistration.Domain
{
    public interface IBidderRepository
    {
        Task Persist(BidderData bidder);
    }

    public class BidderRepository : IBidderRepository
    {
        private CosmosClient _cosmosClient;
        private CosmosDbSettings _cosmosDbSettings;
        private readonly TelemetryClient _telemetryClient;
        private volatile Container _container;
        private static readonly object _lockObject = new();

        public BidderRepository(CosmosDbSettings cosmosDbSettings, TelemetryClient telemetryClient)
        {
            _cosmosDbSettings = cosmosDbSettings;
            _telemetryClient = telemetryClient;
        }


        private static readonly CosmosClientOptions CosmosClientOptions = new()
        {
            MaxRetryAttemptsOnRateLimitedRequests = 2,
            MaxRetryWaitTimeOnRateLimitedRequests = TimeSpan.FromSeconds(1),
            ConsistencyLevel = ConsistencyLevel.Eventual,
            RequestTimeout = TimeSpan.FromMilliseconds(3000),
            SerializerOptions = new CosmosSerializationOptions() { PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase }
        };

        public async Task Persist(BidderData bidder)
        {
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            Exception exception = null;
            try
            {
                await GetContainer().UpsertItemAsync(bidder);
            }
            catch(Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                timer.Stop();
                _telemetryClient.TrackDependency(new DependencyTelemetry("Azure DocumentDB", null, "BuyerRegistrationUpsert", bidder.Id.ToString(), startTime, timer.Elapsed, "", exception == null));
            }
        }

        private Container GetContainer()
        {
            if (_container != null)
                return _container;

            lock (_lockObject)
            {
                if (_container != null)
                    return _container;

                var connectionString = _cosmosDbSettings.ConnectionString;             

                _cosmosClient = new CosmosClient(connectionString, CosmosClientOptions);

                var databaseId = _cosmosDbSettings.Database;
                var containerName = _cosmosDbSettings.ContainerName;

                var database = _cosmosClient.GetDatabase(databaseId);
                _container = database.GetContainer(containerName);
            }
            return _container;
        }
    }
}
