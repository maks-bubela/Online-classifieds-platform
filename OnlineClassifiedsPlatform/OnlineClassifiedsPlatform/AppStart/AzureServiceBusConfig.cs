using OnlineClassifiedsPlatform.AzureServiceBus.Interfaces;
using OnlineClassifiedsPlatform.Configuration;
using System;

namespace OnlineClassifiedsPlatform.AppStart
{
    public class AzureServiceBusConfig
    {
        private const string GoodsPaymentQueueName = "GoodsPaymentQueue";
        private static readonly string AzureQueueNameGoodsPayment = AppSettings.CreateFromConfigurations(GoodsPaymentQueueName).SettingsValue;
        
        public static void Configure(IServiceProvider serviceProvider)
        {
            var queueInitializer = (IServiceBusQueueInitializer)serviceProvider.GetService(typeof(IServiceBusQueueInitializer));
            queueInitializer.CreateIfNotExtistAsync(AzureQueueNameGoodsPayment);

            var topicInitializer = (IServiceBusTopicInitializer)serviceProvider.GetService(typeof(IServiceBusTopicInitializer));
            topicInitializer.CreateTopicIfNotExistsAsync();
            topicInitializer.CreateSubscriptionIfNotExistsAsync(Environment.MachineName);
        }
    }
}
