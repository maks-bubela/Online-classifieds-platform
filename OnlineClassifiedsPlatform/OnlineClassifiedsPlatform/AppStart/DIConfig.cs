﻿using Autofac;
using AutoMapper;
using OnlineClassifiedsPlatform.Autofac.Modules;
using OnlineClassifiedsPlatform.BLL.MappingProfiles;
using OnlineClassifiedsPlatform.Interfaces;
using OnlineClassifiedsPlatform.JwtConfig.Provider;
using OnlineClassifiedsPlatform.MappingProfiles;

namespace OnlineClassifiedsPlatform.AppStart
{
    public class DIConfig
    {
        public static ContainerBuilder Configure(ContainerBuilder containerBuilder)
        {

            containerBuilder.RegisterModule<DALModule>();
            containerBuilder.RegisterModule<ServiceModule>();
            containerBuilder.RegisterType<AuthOptions>().As<IAuthOptions>();
            containerBuilder.Register(ctx => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserProfileBLL());
                cfg.AddProfile(new UserProfile());
            }).CreateMapper()).As<IMapper>().InstancePerLifetimeScope();
            return containerBuilder;
        }
    }
}
