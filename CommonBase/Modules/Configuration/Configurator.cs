//@BaseCode
//MdStart
using Microsoft.Extensions.Configuration;

namespace CommonBase.Modules.Configuration
{
    /// <summary>
    /// Represents a static class for configuring the application.
    /// </summary>
    public static partial class Configurator
    {
        /// <summary>
        /// Initializes the <see cref="Configurator"/> class.
        /// </summary>
        /// <remarks>
        /// This static constructor calls the <see cref="ClassConstructing"/> method before class construction
        /// and the <see cref="ClassConstructed"/> method after class construction.
        /// </remarks>
        static Configurator()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// Event handler that is called before the class is being constructed.
        /// </summary>
        static partial void ClassConstructing();
        /// <summary>
        /// Represents a partial method that is called when a class is constructed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is called at the end of the constructor of a class if it has been implemented.
        /// </para>
        /// <para>
        /// It can be overridden in a partial class to provide additional functionality at class construction.
        /// </para>
        /// </remarks>
        static partial void ClassConstructed();
        /// <summary>
        /// Loads the application settings from the appsettings.json file and environment variables.
        /// </summary>
        /// <returns>The loaded IConfigurationRoot object.</returns>
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


