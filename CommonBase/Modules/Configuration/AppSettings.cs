//@BaseCode
//MdStart
using Microsoft.Extensions.Configuration;

namespace CommonBase.Modules.Configuration
{
    public partial class AppSettings
    {
        static AppSettings()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();

        private static IConfiguration? configuration;

        public static IConfiguration Configuration
        {
            get => configuration ??= Configurator.LoadAppSettings();
            set => configuration = value;
        }

        public static string? Get(string key)
        {
            var result = default(string);

            if (Configuration != null)
            {
                result = Configuration[key];
            }
            return result;
        }
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
