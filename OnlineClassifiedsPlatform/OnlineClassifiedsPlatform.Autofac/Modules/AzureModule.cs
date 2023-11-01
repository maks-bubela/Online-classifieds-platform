using Autofac;
using OnlineClassifiedsPlatform.AzureServiceBus.Interfaces;
using OnlineClassifiedsPlatform.AzureServiceBus.ServiceBusQueue;
using OnlineClassifiedsPlatform.AzureServiceBus.ServiceBusTopic;
using OnlineClassifiedsPlatform.Configuration;

namespace OnlineClassifiedsPlatform.Autofac.Modules
{
    public class AzureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var config = OnlineClassifiedsPlatformConfiguration.CreateFromConfigurations();
            var connectionString = config.AzureServiceBusConnectionString;
            builder.Register(prod => new ServiceBusQueueConsumer(connectionString)).AsSelf().As<IServiceBusQueueConsumer>();
            builder.Register(cons => new ServiceBusQueueProducer(connectionString)).AsSelf().As<IServiceBusQueueProducer>();
            builder.Register(init => new ServiceBusQueueInitializer(connectionString)).AsSelf().As<IServiceBusQueueInitializer>();

            builder.Register(topicInit => new ServiceBusTopicInitializer(connectionString)).AsSelf().As<IServiceBusTopicInitializer>();
            builder.Register(topicInit => new ServiceBusTopicConsumer(connectionString)).AsSelf().As<IServiceBusTopicConsumer>();
            builder.Register(topicInit => new ServiceBusTopicProducer(connectionString)).AsSelf().As<IServiceBusTopicProducer>();
            base.Load(builder);
        }
    }
}