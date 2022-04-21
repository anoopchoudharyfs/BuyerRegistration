using BuyerRegistration.Api.Application.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using BuyerRegistration.Api.Application.Middleware;
using BuyerRegistration.HttpHeaders;

namespace BuyerRegistration.Api.Extensions
{
    /// <summary>
    /// Extension methods on IApplicationBuilder.
    /// Other required extension methods for IApplicationBuilder can be created in this class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Register all custom middleware for IApplicationBuilder here.
        /// </summary>
        /// <param name="app"></param>
        internal static void RegisterMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<SetCorrelationIdMiddleware>();    
            app.UseMiddleware<HeaderValidationMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();

        }


        /// <summary>
        /// Register swagger and its options here.
        /// </summary>
        /// <param name="app"></param>
        internal static void RegisterSwagger(this IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a json endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui.
            app.UseSwaggerUI(swaggerOptions =>
            {
                swaggerOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "BuyerRegistration.Api v1");
                swaggerOptions.DisplayRequestDuration();
            });
        }
    }
}