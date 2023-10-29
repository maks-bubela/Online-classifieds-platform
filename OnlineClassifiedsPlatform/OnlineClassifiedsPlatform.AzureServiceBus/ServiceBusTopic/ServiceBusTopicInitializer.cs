using System;
using System.Threading.Tasks;
using OnlineClassifiedsPlatform.Configuration;
using Microsoft.Azure.ServiceBus.Management;
using OnlineClassifiedsPlatform.AzureServiceBus.Interfaces;

namespace OnlineClassifiedsPlatform.AzureServiceBus.ServiceBusTopic
{
    public class ServiceBusTopicInitializer : IServiceBusTopicInitializer
    {
        private readonly string _connectionString;

        private const string NotifyUserTopicPathName = "NotifyUsersTopicPath";

        private readonly string NotifyUsersTopicPath = AppSettings.CreateFromConfigurations(NotifyUserTopicPathName).SettingsValue;

        private readonly ManagementClient _managementClient;

        public ServiceBusTopicInitializer(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException();
            _managementClient = new ManagementClient(_connectionString) ?? throw new ArgumentNullException();
        }

        public async Task CreateTopicIfNotExistsAsync()
        {
            if (! await _managementClient.TopicExistsAsync(NotifyUsersTopicPath))
            {
                await _managementClient.CreateTopicAsync(NotifyUsersTopicPath);
            }
        }

        public async Task CreateSubscriptionIfNotExistsAsync(string subscriptionName)
        {
            if (! await _managementClient.SubscriptionExistsAsync(NotifyUsersTopicPath, subscriptionName))
            {
                await _managementClient.CreateSubscriptionAsync(NotifyUsersTopicPath, subscriptionName);
            }
        }
    }
}