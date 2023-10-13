using OnlineClassifiedsPlatform.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.BLL.Interfaces
{
    public interface IAccountService
    {
        Task<long> RegisterUserAsync(UserDTO userDTO);
    }
}
