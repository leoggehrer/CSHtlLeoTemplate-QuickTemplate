//@BaseCode
//MdStart
using Microsoft.Extensions.Configuration;

namespace CommonBase.Modules.Configuration
{
    public static partial class Configurator
    {
        static Configurator()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        public static IConfigurationRoot LoadAppSettings()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName ?? "Development"}.json", optional: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
//MdEnd
