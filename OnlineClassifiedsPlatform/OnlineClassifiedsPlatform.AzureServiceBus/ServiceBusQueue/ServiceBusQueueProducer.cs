using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using OnlineClassifiedsPlatform.AzureServiceBus.Interfaces;

namespace OnlineClassifiedsPlatform.AzureServiceBus.ServiceBusQueue
{
    public class ServiceBusQueueProducer : IServiceBusQueueProducer
    {
        private readonly string _connectionString;

        public ServiceBusQueueProducer(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(_connectionString));
        }

        public async Task SendMessageAsync<T>(T message, string queueName, DateTime? scheduleTime = null) where T : class
        {
            var client = new ServiceBusClient(_connectionString);
            var sender = client.CreateSender(queueName);
            var serviceBusMessage = new ServiceBusMessage(JsonConvert.SerializeObject(message));
            if (scheduleTime != null)
            {
                serviceBusMessage.ScheduledEnqueueTime = (DateTimeOffset)scheduleTime;
            }
            await sender.SendMessageAsync(serviceBusMessage);
        }
    }
}