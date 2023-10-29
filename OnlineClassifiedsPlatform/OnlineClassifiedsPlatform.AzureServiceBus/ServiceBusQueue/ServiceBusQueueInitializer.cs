using System;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus.Management;
using OnlineClassifiedsPlatform.AzureServiceBus.Interfaces;

namespace OnlineClassifiedsPlatform.AzureServiceBus.ServiceBusQueue
{
    public class ServiceBusQueueInitializer : IServiceBusQueueInitializer
    {
        private readonly string _connectionString;

        public ServiceBusQueueInitializer(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(_connectionString));
        }

        public async Task CreateIfNotExtistAsync(string queueName)
        {
            var _managementClient = new ManagementClient(_connectionString) ?? throw new ArgumentNullException();
            if (!await _managementClient.QueueExistsAsync(queueName))
            {
                await _managementClient.CreateQueueAsync(queueName);
            }
        }
    }
}