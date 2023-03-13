using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramsTask.Helpers.Config
{
    public static class ConfigurationSettingsHelper
    {
        private static IConfiguration? _Configuration { get; set; }

        public static IConfiguration Configuration
        {
            set
            {
                _Configuration = value;
            }
        }

        public static T GetConfigurationSectionObject<T>(string? configurationSectionPropertyName)
        {
            return _Configuration.GetSection(configurationSectionPropertyName).Get<T>();
        }
    }
}
