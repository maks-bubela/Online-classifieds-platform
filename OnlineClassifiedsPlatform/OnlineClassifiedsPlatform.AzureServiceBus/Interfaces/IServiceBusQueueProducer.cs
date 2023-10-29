using System;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.AzureServiceBus.Interfaces
{
    public interface IServiceBusQueueProducer
    {
        Task SendMessageAsync<T>(T message, string queueName, DateTime? scheduleTime = null) where T : class;
    }
}