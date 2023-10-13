using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace OnlineClassifiedsPlatform.Configuration
{
    public class OnlineClassifiedsPlatformConfiguration
    {
        public const string SqlConnectionStringSectionName = "ConnectionStrings:OnlineClassifiedsPlatformDB";


        public string SqlConnectionString { get; private set; }


        public OnlineClassifiedsPlatformConfiguration(string sqlConnectionString)
        {
            if (string.IsNullOrWhiteSpace(nameof(sqlConnectionString)))
            {
                throw new ArgumentException(nameof(sqlConnectionString));
            }

            SqlConnectionString = sqlConnectionString;
        }

        public static OnlineClassifiedsPlatformConfiguration CreateFromConfigurations()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            IConfigurationRoot root = configurationBuilder.Build();

            return new OnlineClassifiedsPlatformConfiguration(
                sqlConnectionString: root.GetSection(SqlConnectionStringSectionName).Value);
        }
    }
}
