using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.AzureServiceBus.Interfaces
{
    public interface IServiceBusTopicProducer
    {
        Task SendMessageAsync<T>(T model, string topicPath) where T : class;
    }
}