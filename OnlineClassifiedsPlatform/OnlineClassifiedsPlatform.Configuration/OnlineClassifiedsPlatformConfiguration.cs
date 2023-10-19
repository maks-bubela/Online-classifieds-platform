using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace OnlineClassifiedsPlatform.Configuration
{
    public class OnlineClassifiedsPlatformConfiguration
    {
        public const string SqlConnectionStringSectionName = "ConnectionStrings:OnlineClassifiedsPlatformDB";
        public const string AzureStorageConnectionStringSectionName = "ConnectionStrings:AzureStorage";


        public string SqlConnectionString { get; private set; }
        public string AzureStorageConnectionString { get; private set; }


        public OnlineClassifiedsPlatformConfiguration(string sqlConnectionString, string azureStorageConnectionString)
        {
            if (string.IsNullOrWhiteSpace(nameof(sqlConnectionString)))
            {
                throw new ArgumentException(nameof(sqlConnectionString));
            }
            if (string.IsNullOrWhiteSpace(nameof(azureStorageConnectionString)))
            {
                throw new ArgumentException(nameof(azureStorageConnectionString));
            }

            SqlConnectionString = sqlConnectionString;
            AzureStorageConnectionString = azureStorageConnectionString;
        }

        public static OnlineClassifiedsPlatformConfiguration CreateFromConfigurations()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            IConfigurationRoot root = configurationBuilder.Build();

            return new OnlineClassifiedsPlatformConfiguration(
                sqlConnectionString: root.GetSection(SqlConnectionStringSectionName).Value,
                azureStorageConnectionString: root.GetSection(AzureStorageConnectionStringSectionName).Value);
        }
    }
}
