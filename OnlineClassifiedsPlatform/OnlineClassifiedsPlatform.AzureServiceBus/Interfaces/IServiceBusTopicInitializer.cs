using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.AzureServiceBus.Interfaces
{
    public interface IServiceBusTopicInitializer
    {
        Task CreateTopicIfNotExistsAsync();
        Task CreateSubscriptionIfNotExistsAsync(string subscriptionName);
    }
}