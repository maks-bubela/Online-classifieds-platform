using OnlineClassifiedsPlatform.AzureServiceBus.Interfaces;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.Tests.Stubs
{
    public class ServiceBusTopicProducerStub : IServiceBusTopicProducer
    {
        public async Task SendMessageAsync<T>(T model, string topicPath) where T : class
        {
            Console.WriteLine(JsonConvert.SerializeObject(model));
        }
    }
}
