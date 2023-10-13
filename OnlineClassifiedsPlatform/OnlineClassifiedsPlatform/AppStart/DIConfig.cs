using Autofac;
using AutoMapper;
using OnlineClassifiedsPlatform.Autofac.Modules;
using OnlineClassifiedsPlatform.BLL.MappingProfiles;

namespace OnlineClassifiedsPlatform.AppStart
{
    public class DIConfig
    {
        public static ContainerBuilder Configure(ContainerBuilder containerBuilder)
        {

            containerBuilder.RegisterModule<DALModule>();
            containerBuilder.Register(ctx => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserProfileBLL());
            }).CreateMapper()).As<IMapper>().InstancePerLifetimeScope();
            return containerBuilder;
        }
    }
}
