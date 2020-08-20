using System;
using Microsoft.Extensions.Configuration;

namespace SyaApi.Plugins
{
    public static class ConfigReader
    {
        private static IConfiguration configuration { get; set; }

        static ConfigReader()
        {
            var path = $"{AppContext.BaseDirectory}config.json";
            var builder = new ConfigurationBuilder().AddJsonFile(path, false, true);

            configuration = builder.Build();
        }

        public static string GetConfig(string configName)
        {
            return configuration[configName];
        }
    }
}
