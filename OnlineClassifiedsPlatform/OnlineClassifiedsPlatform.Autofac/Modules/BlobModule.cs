using Autofac;
using OnlineClassifiedsPlatform.AzureStorage.BlobStorage;
using OnlineClassifiedsPlatform.AzureStorage.Interfaces;
using OnlineClassifiedsPlatform.Configuration;

namespace OnlineClassifiedsPlatform.Autofac.Modules
{
    public class BlobModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var config = OnlineClassifiedsPlatformConfiguration.CreateFromConfigurations();
            var connectionString = config.AzureStorageConnectionString;
            builder.Register(prov => new BlobProvider(connectionString)).AsSelf().As<IBlobProvider>();
            base.Load(builder);
        }
    }
}
