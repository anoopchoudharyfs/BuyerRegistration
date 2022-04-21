using System.Reflection;

namespace BuyerRegistration.Api.Domain.Model
{
    public static class HealthCheckResponse
    {
        public static string BuildVersion { get; set; }
        static HealthCheckResponse()
        {
            BuildVersion = GetBuildVersion();
        }

        public static string GetBuildVersion()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            AssemblyInformationalVersionAttribute versionAttribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            string buildVersion = versionAttribute.InformationalVersion;
            return buildVersion;
        }
    }
}
