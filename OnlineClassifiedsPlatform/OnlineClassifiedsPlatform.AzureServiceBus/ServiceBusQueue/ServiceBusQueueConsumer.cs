using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using OnlineClassifiedsPlatform.AzureServiceBus.Interfaces;

namespace OnlineClassifiedsPlatform.AzureServiceBus.ServiceBusQueue
{
    public class ServiceBusQueueConsumer : IServiceBusQueueConsumer
    {
        private readonly string _connectionString;

        public ServiceBusQueueConsumer(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(_connectionString));
        }

        public async Task<T> GetMessageAsync<T>(string queueName, Action<T> action) where T : class
        {
            var queueServiceClient = new ServiceBusClient(_connectionString);
            var options = new ServiceBusReceiverOptions() { ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete };
            var receiver = queueServiceClient.CreateReceiver(queueName, options);
            string body;
            while (true)
            {
                var receivedMessage = await receiver.ReceiveMessageAsync();
                if (receivedMessage != null && receivedMessage.Body != null)
                {
                    body = receivedMessage.Body.ToString();
                    action(JsonConvert.DeserializeObject<T>(body));
                }
            }
        }
    }
}