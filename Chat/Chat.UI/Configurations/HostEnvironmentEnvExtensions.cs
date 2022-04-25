using System.Diagnostics.CodeAnalysis;

namespace Chat.UI.Configurations
{
    [ExcludeFromCodeCoverage]
    internal static class HostEnvironmentEnvExtensions
    {
        public static bool IsDevelopmentOrDocker(this IHostEnvironment hostEnvironment)
        {
            return hostEnvironment.IsDevelopment() || hostEnvironment.IsEnvironment("Docker");
        }
    }
}