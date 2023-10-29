using System;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using OnlineClassifiedsPlatform.AzureServiceBus.Interfaces;

namespace OnlineClassifiedsPlatform.AzureServiceBus.ServiceBusTopic
{
    public class ServiceBusTopicProducer : IServiceBusTopicProducer
    {
        private readonly string _connectionString;
        public ServiceBusTopicProducer(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException();
        }

        public async Task SendMessageAsync<T>(T model, string topicPath) where T : class
        {
            var topicClient = new TopicClient(_connectionString, topicPath);
            await topicClient.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model))));
        }
    }
}