using OnlineClassifiedsPlatform.BLL.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.BLL.Interfaces
{
    public interface ITokenService
    {
        Task<int> GetTokenSettingsAsync(EnvirementTypes type);
    }
}
