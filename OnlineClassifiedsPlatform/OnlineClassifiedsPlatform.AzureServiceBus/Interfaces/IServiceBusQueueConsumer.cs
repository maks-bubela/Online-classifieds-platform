using System;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.AzureServiceBus.Interfaces
{
    public interface IServiceBusQueueConsumer
    {
        Task<T> GetMessageAsync<T>(string queueName, Action<T> action) where T : class;
    }
}
