using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace OnlineClassifiedsPlatform.AzureServiceBus.Interfaces
{
    public interface IServiceBusTopicConsumer
    {
        Task<SubscriptionClient> GetSubscriptionClient(string topicPathName);
    }
}