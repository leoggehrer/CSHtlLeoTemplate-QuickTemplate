//@BaseCode
//MdStart
using Microsoft.Extensions.Configuration;

namespace CommonBase.Modules.Configuration
{
    /// <summary>
    /// Represents a class that provides access to application settings.
    /// </summary>
    public partial class AppSettings
    {
        /// <summary>
        /// Initializes the static <see cref="AppSettings"/> class.
        /// </summary>
        /// <remarks>
        /// This static constructor is called only once, before the first instance of the class is created.
        /// </remarks>
        static AppSettings()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// This method is called before the construction of the class.
        /// </summary>
        /// <remarks>
        /// Use this method to perform any initializations or checks required before the class is fully constructed.
        /// </remarks>
        static partial void ClassConstructing();
        /// <summary>
        /// A partial method that is automatically called when an instance of the class is constructed.
        /// This method is not meant to be implemented by the developer, but can be defined in other parts of the class.
        /// </summary>
        static partial void ClassConstructed();
        private static IConfiguration? configuration;

        /// <summary>
        /// Gets or sets the configuration settings.
        /// </summary>
        /// <value>
        /// The configuration settings.
        /// </value>
        public static IConfiguration Configuration
        {
            get => configuration ??= Configurator.LoadAppSettings();
            set => configuration = value;
        }

        /// <summary>
        /// Retrieves the value associated with the specified key from the Configuration.
        /// </summary>
        /// <param name="key">The key representing the value to retrieve from the Configuration.</param>
        /// <returns>The value associated with the specified key, or null if the Configuration is null or the key does not exist.</returns>
        public static string? Get(string key)
        {
            var result = default(string);

            if (Configuration != null)
            {
                result = Configuration[key];
            }
            return result;
        }
        /// <summary>
        /// Gets the configuration section with the specified key.
        /// </summary>
        /// <param name="key">The key of the configuration section to retrieve.</param>
        /// <returns>The configuration section with the specified key, or null if not found.</returns>
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


