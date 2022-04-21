using System;
using Microsoft.ApplicationInsights;

namespace BuyerRegistration.Services
{
    public interface ITelemetryLogger
    {
        void LogException(Exception exception);
    }

    public class TelemetryLogger : ITelemetryLogger
    {
        private readonly TelemetryClient _telemetryClient;

        public TelemetryLogger(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        public void LogException(Exception exception)
        {
            _telemetryClient.TrackException(exception);
        }
    }
}
