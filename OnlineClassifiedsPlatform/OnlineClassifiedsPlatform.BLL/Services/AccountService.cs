using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineClassifiedsPlatform.BLL.DTO;
using OnlineClassifiedsPlatform.BLL.Enums;
using OnlineClassifiedsPlatform.BLL.Exceptions;
using OnlineClassifiedsPlatform.BLL.Interfaces;
using OnlineClassifiedsPlatform.DAL;
using OnlineClassifiedsPlatform.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly OnlineClassifiedsPlatformContext _ctx;
        private readonly IPasswordProcessing _passProcess;

        public AccountService(OnlineClassifiedsPlatformContext ctx, IMapper mapper,
            IPasswordProcessing passProcess, IUserService userService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
            _passProcess = passProcess ?? throw new ArgumentNullException(nameof(passProcess));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        #region Create methods
        public async Task<long> RegisterUserAsync(UserDTO userDTO)
        {
            if (userDTO == null) throw new ArgumentException(nameof(userDTO));
            if (await _userService.UserExistsAsync(userDTO.Id)) throw new DataExistsInDatabaseException();

            var salt = _passProcess.GenerateSalt();
            var role = await _ctx.Set<Role>().Where(x => x.Name == userDTO.RoleName)
                .SingleOrDefaultAsync();
            if (role == null)
            {
                role = await _ctx.Set<Role>().Where(x => x.Id == (long)Roles.Customer)
                    .SingleOrDefaultAsync() ?? throw new NullReferenceException(nameof(role));
            }

            var user = _mapper.Map<User>(userDTO);
            user.Password = _passProcess.GetHashCode(userDTO.Password, salt);
            user.Salt = salt;
            user.RoleId = role.Id;
            user.IsDelete = false;

            _ctx.Set<User>().Add(user);
            await _ctx.SaveChangesAsync();
            if (!await _userService.UserExistsAsync(user.Id)) throw new FailedAddToDatabaseException();
            return user.Id;
        }
        #endregion
    }
}