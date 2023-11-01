using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineClassifiedsPlatform.DAL.Context;
using OnlineClassifiedsPlatform.ExtensionMethods;
using OnlineClassifiedsPlatform.Filters;
using OnlineClassifiedsPlatform.SignalR.Hubs;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace OnlineClassifiedsPlatform.AppStart
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR()
                .AddHubOptions<GoodsNotificationHub>(options =>
                {
                    options.EnableDetailedErrors = true;
                });
            services.AddControllersWithViews();
            services.AddSwagger();
            services.AddControllersWithViews();
            services.AddJwtToken(Configuration);
            services.AddOptions();
            services.AddMvc(options => options.Filters.Add(new ExceptionFilter()));
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            DIConfig.Configure(builder);
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<OnlineClassifiedsPlatformContext>();
                context.Database.EnsureCreated();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            AzureServiceBusConfig.Configure(app.ApplicationServices);
            app.UseCustomSwagger();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();

                endpoints.MapControllerRoute(
                    name: "swagger",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapHub<GoodsNotificationHub>("/goods", options =>
                {
                    options.Transports =
                        HttpTransportType.WebSockets |
                        HttpTransportType.LongPolling;
                });
            });
        }
    }
}
