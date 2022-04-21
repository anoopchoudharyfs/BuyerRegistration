using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using BuyerRegistration.Api.Domain.Model;

namespace BuyerRegistration.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v1")]
    [ExcludeFromCodeCoverage]
    public class HealthController : ControllerBase
    { 
        /// <summary>
        /// Health check endpoint
        /// This health endpoint will ensure health for all the external services that the service is using which are not registered with Service Discovery.
        /// </summary>
        /// <response code="200"> Success </response>
        [HttpGet]
        [Route("health")]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status200OK)]
        public async Task<ActionResult> HealthCheck()
        {
            await Task.CompletedTask;
            return Ok(HealthCheckResponse.BuildVersion);
        }
    }
}