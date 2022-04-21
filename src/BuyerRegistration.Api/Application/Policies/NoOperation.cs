using Polly;
using System.Net.Http;

namespace BuyerRegistration.Api.Application.Policies
{
    public static partial class Polly
    {
        public static IAsyncPolicy<HttpResponseMessage> GetNoOperationPolicy()
        {
            return Policy.NoOpAsync<HttpResponseMessage>();
        }
    }
}