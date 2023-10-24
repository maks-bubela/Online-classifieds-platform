using Autofac;
using OnlineClassifiedsPlatform.BLL.Cryptography;
using OnlineClassifiedsPlatform.BLL.Interfaces;
using OnlineClassifiedsPlatform.BLL.Services;


namespace OnlineClassifiedsPlatform.Autofac.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<AccountService>().As<IAccountService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<TokenService>().As<ITokenService>();
            builder.RegisterType<PasswordProcessing>().As<IPasswordProcessing>();
            builder.RegisterType<AzureStorageService>().As<IAzureStorageService>();
            builder.RegisterType<GoodsService>().As<IGoodsService>();
            builder.RegisterType<FileUploadService>().As<IFileUploadService>();

            base.Load(builder);
        }
    }
}
