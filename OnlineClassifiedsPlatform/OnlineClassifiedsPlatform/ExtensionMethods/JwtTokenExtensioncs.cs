using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OnlineClassifiedsPlatform.JwtConfig.JwtSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.ExtensionMethods
{
    public static class JwtTokenExtension
    {
        private const string JwtSettingsName = "JwtSettings";
        private const string JwtSettingsIssuer = "JwtSettings:Issuer";
        private const string JwtSettingsAudience = "JwtSettings:Audience";
        private const string JwtSettingsKey = "JwtSettings:Key";
        public static void AddJwtToken(this IServiceCollection services, IConfigurationRoot Configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidIssuer = Configuration.GetValue<string>(JwtSettingsIssuer),
                        ValidateAudience = false,
                        ValidAudience = Configuration.GetValue<string>(JwtSettingsAudience),
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetValue<string>(JwtSettingsKey))),
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });
            services.Configure<JwtSetting>(Configuration.GetSection(JwtSettingsName));
        }
    }
}
