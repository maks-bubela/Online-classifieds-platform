using OnlineClassifiedsPlatform.BLL.DTO;
using OnlineClassifiedsPlatform.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.BLL.Interfaces
{
    public interface IUserService
    {
        Task<bool> SoftDeleteAsync(long id);
        Task<bool> UserExistsAsync(long id);
        Task<UserDTO> GetUserByIdAsync(long id);
        Task<UserDTO> GetUserByUsernameAsync(string username);
        Task<List<UserDTO>> GetUsersListAsync();

        Expression<Func<User, bool>> CreateActiveUserPredicate(long userId);
    }
}
