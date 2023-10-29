using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.AzureServiceBus.Interfaces
{
    public interface IServiceBusQueueInitializer
    {
        Task CreateIfNotExtistAsync(string queueName);
    }
}