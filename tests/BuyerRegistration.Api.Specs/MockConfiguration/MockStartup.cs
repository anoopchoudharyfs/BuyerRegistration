using System.Net.Http;
using BuyerRegistration.Api;
using BuyerRegistration.Domain.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Moq;

namespace BuyerRegistration.Specs.MockConfiguration
{
    public class MockStartup : Startup
    {
        private readonly IConfiguration _configuration;
        public MockStartup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment) : base(configuration, webHostEnvironment)
        {
            _configuration = configuration;
            UseAppConfiguration = false;
        }

        public override void Configure(IApplicationBuilder app)
        {
            base.Configure(app);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
           
            // Service discovery client and in memory cache integration
            services.AddMemoryCache();
            services.AddFeatureManagement();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            services.AddSingleton(provider => httpClientFactory.Object);

        }
    }
}