using System;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using OnlineClassifiedsPlatform.AzureServiceBus.Interfaces;

namespace OnlineClassifiedsPlatform.AzureServiceBus.ServiceBusTopic
{
    public class ServiceBusTopicConsumer : IServiceBusTopicConsumer
    {
        private string _connectionString;
        private readonly ManagementClient _managementClient;
        private readonly string MachineName = Environment.MachineName;

        public ServiceBusTopicConsumer(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(_connectionString));
            _managementClient = new ManagementClient(_connectionString);
        }

        public async Task<SubscriptionClient> GetSubscriptionClient(string topicPath)
        {
            if (await _managementClient.SubscriptionExistsAsync(topicPath, MachineName))
            {
                var subscriptionClient = new SubscriptionClient(_connectionString, topicPath, MachineName);
                return subscriptionClient;
            }
            return null;
        }
    }
}