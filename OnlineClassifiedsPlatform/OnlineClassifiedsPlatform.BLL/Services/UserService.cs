using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineClassifiedsPlatform.BLL.DTO;
using OnlineClassifiedsPlatform.BLL.Exceptions;
using OnlineClassifiedsPlatform.BLL.Interfaces;
using OnlineClassifiedsPlatform.DAL;
using OnlineClassifiedsPlatform.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly OnlineClassifiedsPlatformContext _ctx;

        private const long ID_NOT_FOUND = 0;

        public UserService(IMapper mapper, OnlineClassifiedsPlatformContext ctx)
        {
            _mapper = mapper ?? throw new ArgumentNullException();
            _ctx = ctx ?? throw new ArgumentNullException();
        }

        #region Predicates
        public Expression<Func<User, bool>> CreateActiveUserPredicate(long userId)
        {
            Expression<Func<User, bool>> predicate = x => x.Id == userId && !x.IsDelete;
            return predicate;
        }

        #endregion

        #region Read Methods
        public async Task<UserDTO> GetUserByIdAsync(long id)
        {
            if (id <= ID_NOT_FOUND) throw new EntityArgumentNullException(nameof(id));

            var predicate = CreateActiveUserPredicate(id);
            var user = await _ctx.Set<User>().Where(predicate)
                .Include(x => x.Role)
                .SingleOrDefaultAsync();
            if (user == null) throw new ArgumentNullException(nameof(user));

            var userDTO = _mapper.Map<UserDTO>(user);
            return userDTO;
        }

        public async Task<UserDTO> GetUserByUsernameAsync(string username)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));

            var user = await _ctx.Set<User>().Include(x => x.Role)
                .Where(x => x.Username == username)
                .SingleOrDefaultAsync();
            if (user == null) throw new NullReferenceException(nameof(user));

            var userDTO = _mapper.Map<UserDTO>(user);
            return userDTO;
        }

        public async Task<List<UserDTO>> GetUsersListAsync()
        {
            var usersList = await _ctx.Set<User>().Include(x => x.Role)
                .Where(x => !x.IsDelete).ToListAsync();
            if (usersList == null) throw new NullReferenceException(nameof(usersList));

            var userDTOList = _mapper.Map<List<UserDTO>>(usersList);
            return userDTOList;
        }

        public async Task<bool> UserExistsAsync(long id)
        {
            var predicate = CreateActiveUserPredicate(id);
            var exists = await _ctx.Set<User>().AnyAsync(predicate);
            return exists;
        }
        #endregion

        #region Delete Methods
        public async Task<bool> SoftDeleteAsync(long id)
        {
            if (!await UserExistsAsync(id)) throw new NotFoundInDatabaseException();

            var predicate = CreateActiveUserPredicate(id);
            var user = await _ctx.Set<User>().Where(predicate)
                .SingleOrDefaultAsync();
            if (user != null)
            {
                user.IsDelete = true;
                _ctx.Entry(user).State = EntityState.Modified;
                await _ctx.SaveChangesAsync();
                return true;
            }
            return false;
        }
        #endregion
    }
}
