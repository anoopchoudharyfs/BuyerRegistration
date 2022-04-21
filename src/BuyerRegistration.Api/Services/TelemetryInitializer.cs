using BuyerRegistration.Api.Services.IService;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace BuyerRegistration.Api.Services
{
    public class TelemetryInitializer : ITelemetryInitializer
    {
        private const string CorrelationIdKey = "x-bid-correlation-id";
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public TelemetryInitializer(ICorrelationIdProvider correlationIdProvider)
        {
            _correlationIdProvider = correlationIdProvider;
        }
        public void Initialize(ITelemetry telemetry)
        {
            var propTelemetry = (ISupportProperties)telemetry;
            if (!propTelemetry.Properties.ContainsKey(CorrelationIdKey))
            {
                propTelemetry.Properties.Add(CorrelationIdKey, _correlationIdProvider.GetCorrelationId());
            }
        }
    }
}
