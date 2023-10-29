//@BaseCode
//MdStart
using Microsoft.Extensions.Configuration;

namespace QuickTemplate.Logic.Modules.Configuration
{
    /// <summary>
    /// Provides access to application settings stored in the configuration.
    /// </summary>
    public static partial class AppSettings
    {
        private static IConfiguration? configuration;
        
        /// <summary>
        /// Gets or sets the application configuration.
        /// </summary>
        private static IConfiguration Configuration
        {
            get => configuration ??= CommonBase.Modules.Configuration.Configurator.LoadAppSettings();
            set => configuration = value;
        }
        
        /// <summary>
        /// Sets the configuration object for the application.
        /// </summary>
        /// <param name="configuration">The configuration object to set.</param>
        public static void SetConfiguration(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        /// <summary>
        /// Retrieves the value associated with the specified key from the configuration.
        /// If the key is not found, an empty string is returned.
        /// </summary>
        /// <param name="key">A string representing the key of the configuration value to retrieve.</param>
        /// <returns>A string value representing the configuration value associated with the specified key, or an empty string if not found.</returns>
        public static string Get(string key)
        {
            var result = default(string);
            
            if (Configuration != null)
            {
                result = Configuration[key];
            }
            return result ?? string.Empty;
        }
        /// <summary>
        /// Retrieves a value from the configuration using the specified key, or returns a default value if not found.
        /// </summary>
        /// <param name="key">The key of the value to retrieve from the configuration.</param>
        /// <param name="defaultValue">The default value to return if the key is not found in the configuration.</param>
        /// <returns>The value associated with the specified key if found; otherwise, the default value.</returns>
        public static string Get(string key, string defaultValue)
        {
            var result = defaultValue;
            
            if (Configuration != null)
            {
                result = Configuration[key] ?? defaultValue;
            }
            return result;
        }
        /// <summary>
        /// Retrieves the configuration section associated with the specified key.
        /// </summary>
        /// <param name="key">The key associated with the configuration section to retrieve.</param>
        /// <returns>
        /// Returns the configuration section associated with the specified key.
        /// Returns null if the provided key is not found or the configuration is not available.
        /// </returns>
        public static IConfigurationSection? GetSection(string key)
        {
            var result = default(IConfigurationSection);
            
            if (Configuration != null)
            {
                result = Configuration.GetSection(key);
            }
            return result;
        }
    }
}
//MdEnd

