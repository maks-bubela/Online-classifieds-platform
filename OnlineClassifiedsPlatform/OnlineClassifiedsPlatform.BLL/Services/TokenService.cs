using Microsoft.EntityFrameworkCore;
using OnlineClassifiedsPlatform.BLL.Enums;
using OnlineClassifiedsPlatform.BLL.Exceptions;
using OnlineClassifiedsPlatform.BLL.Interfaces;
using OnlineClassifiedsPlatform.DAL.Context;
using OnlineClassifiedsPlatform.DAL.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.BLL.Services
{
    public class TokenService : ITokenService
    {
        private readonly OnlineClassifiedsPlatformContext _ctx;
        public TokenService(OnlineClassifiedsPlatformContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(OnlineClassifiedsPlatformContext));
        }
        public async Task<int> GetTokenSettingsAsync(EnvirementTypes type)
        {
            var tokenSetting = await _ctx.Set<BearerTokenSetting>().Include(x => x.EnvironmentType)
                .Where(b => b.EnvironmentType.Id == ((long)type))
                .SingleOrDefaultAsync() ?? throw new EntityArgumentNullException(nameof(BearerTokenSetting));
            return tokenSetting.LifeTime;
        }
    }
}
