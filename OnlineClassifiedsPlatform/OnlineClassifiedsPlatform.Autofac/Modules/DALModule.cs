using Autofac;
using OnlineClassifiedsPlatform.Configuration;
using OnlineClassifiedsPlatform.DAL.Context;
using OnlineClassifiedsPlatform.DAL.Interfaces;
using OnlineClassifiedsPlatform.DAL.Repository;

namespace OnlineClassifiedsPlatform.Autofac.Modules
{
    public class DALModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var connectionString = OnlineClassifiedsPlatformConfiguration.CreateFromConfigurations().SqlConnectionString;

            builder.Register(ctx => new OnlineClassifiedsPlatformContext(connectionString)).AsSelf();
            builder.RegisterType<GenericRepository>().As<IGenericRepository>();

            base.Load(builder);
        }
    }
}
