using Bogus;
using System;
using AutoMapper;
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineClassifiedsPlatform.BLL.DTO;
using OnlineClassifiedsPlatform.DAL.Context;
using OnlineClassifiedsPlatform.DAL.Entities;
using OnlineClassifiedsPlatform.BLL.Services;
using OnlineClassifiedsPlatform.BLL.Interfaces;
using OnlineClassifiedsPlatform.BLL.Exceptions;
using OnlineClassifiedsPlatform.BLL.Cryptography;
using OnlineClassifiedsPlatform.BLL.MappingProfiles;

namespace OnlineClassifiedsPlatform.Tests.UnitTests.BLLTests
{
    [TestFixture]
    public class UserServiceTest
    {
        private IMapper _mapper;
        private UserDTO _userDTO;
        private IUserService _userService;
        private IAccountService _accService;
        private OnlineClassifiedsPlatformContext _ctx;
        private IPasswordProcessing _passProcessing;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<OnlineClassifiedsPlatformContext>();
            optionsBuilder.UseInMemoryDatabase(nameof(OnlineClassifiedsPlatformContext));
            _ctx = new OnlineClassifiedsPlatformContext(optionsBuilder.Options);
            _passProcessing = new PasswordProcessing();

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new UserProfileBLL()));
            _mapper = new Mapper(configuration);

            _userService = new UserService(_mapper, _ctx);
            _accService = new AccountService(_ctx, _mapper, _passProcessing, _userService);
            _ctx.Database.EnsureCreated();

            var roles = new[] { "customer", "admin", "staff" };
            _userDTO = new Faker<UserDTO>()
                .RuleFor(x => x.Username, r => r.Internet.UserName())
                .RuleFor(x => x.Password, r => r.Internet.Password(6, false, "", ""))
                .RuleFor(x => x.Firstname, r => r.Person.FirstName)
                .RuleFor(x => x.Lastname, r => r.Person.LastName)
                .RuleFor(x => x.RoleName, r => r.PickRandom(roles)).Generate();
        }

        #region Soft Delete Tests
        [Test, Order(0)]
        public async Task SoftDeleteAsync_RemovingExistingUser_ReturnsTrue()
        {
            await _accService.RegisterUserAsync(_userDTO);
            var newUser = await _ctx.Set<User>().Where(x => x.Username == _userDTO.Username)
                .SingleOrDefaultAsync();
            _userDTO = _mapper.Map<UserDTO>(newUser);

            var isDeleted = await _userService.SoftDeleteAsync(_userDTO.Id);
            Assert.IsTrue(isDeleted);
        }

        [Test, Order(1)]
        public async Task SoftDeleteAsync_RemovingNotExistingUser_ReturnsFalse()
        {
            Assert.ThrowsAsync<NotFoundInDatabaseException>(async () =>
                await _userService.SoftDeleteAsync(0));
        }

        [Test, Order(2)]
        public async Task SoftDeleteAsync_RemovingAlreadyDeletedUser_ReturnsFalse()
        {
            await _accService.RegisterUserAsync(_userDTO);
            var newUser = await _ctx.Set<User>().Where(x => x.Username == _userDTO.Username)
                .SingleOrDefaultAsync();
            newUser.IsDelete = true;
            await _ctx.SaveChangesAsync();

            _userDTO = _mapper.Map<UserDTO>(newUser);
            Assert.ThrowsAsync<NotFoundInDatabaseException>(async () =>
                await _userService.SoftDeleteAsync(_userDTO.Id));
        }
        #endregion

        #region GetUserById Tests
        [Test, Order(3)]
        public async Task GetUserByIdAsync_GettingUserInfoById_ReturnsTrue()
        {
            await _accService.RegisterUserAsync(_userDTO);
            var newUser = await _ctx.Set<User>().Where(x => x.Username == _userDTO.Username)
                .SingleOrDefaultAsync();
            _userDTO = _mapper.Map<UserDTO>(newUser);

            var userInfoDTO = await _userService.GetUserByIdAsync(_userDTO.Id);
            Assert.NotNull(userInfoDTO);
        }

        [Test, Order(4)]
        public async Task GetUserByIdAsync_GettingNotExistingUserInfoById_ThrowsArgumentNullException()
        {
            Assert.ThrowsAsync<EntityArgumentNullException>(async () => 
                await _userService.GetUserByIdAsync(0));
        }
        #endregion

        #region GetUserByUsernameAsync Tests
        [Test, Order(5)]
        public async Task GetUserByUsernameAsync_GettingUserInfo_ReturnsTrue()
        {
            await _accService.RegisterUserAsync(_userDTO);
            var newUser = await _ctx.Set<User>().Where(x => x.Username == _userDTO.Username)
                .SingleOrDefaultAsync();
            _userDTO = _mapper.Map<UserDTO>(newUser);
            var userInfo = await _userService.GetUserByUsernameAsync(_userDTO.Username);
            Assert.NotNull(userInfo);
        }

        [Test, Order(6)]
        public async Task GetUserByUsernameAsync_GettingNotExistingUserInfo_ThrowsNullReferenceException()
        {
            await _accService.RegisterUserAsync(_userDTO);
            var newUser = await _ctx.Set<User>().Where(x => x.Username == _userDTO.Username)
                .SingleOrDefaultAsync();
            _userDTO = _mapper.Map<UserDTO>(newUser);
            
            Assert.ThrowsAsync<NullReferenceException>(async () => 
                await _userService.GetUserByUsernameAsync(username: "notexisting"));
        }

        [Test, Order(7)]
        public async Task GetUserByUsernameAsync_PuttingNullArgument_ThrowsArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => 
                await _userService.GetUserByUsernameAsync(null));
        }
        #endregion

        #region UserExistsAsync Tests
        [Test, Order(8)]
        public async Task UserExistsAsync_PuttingExistingId_ReturnsTrue()
        {
            await _accService.RegisterUserAsync(_userDTO);
            var newUser = await _ctx.Set<User>().Where(x => x.Username == _userDTO.Username)
                .SingleOrDefaultAsync();
            var exists = await _userService.UserExistsAsync(newUser.Id);

            Assert.IsTrue(exists);
        }

        [Test, Order(8)]
        public async Task UserExistsAsync_PuttingNotExistingId_ReturnsFalse()
        {
            var exists = await _userService.UserExistsAsync(0);
            Assert.IsFalse(exists);
        }
        #endregion
    }
}
