using Bogus;
using AutoMapper;
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using OnlineClassifiedsPlatform.BLL.DTO;
using OnlineClassifiedsPlatform.DAL.Context;
using OnlineClassifiedsPlatform.DAL.Entities;
using OnlineClassifiedsPlatform.BLL.Services;
using OnlineClassifiedsPlatform.BLL.Interfaces;
using OnlineClassifiedsPlatform.BLL.Cryptography;
using OnlineClassifiedsPlatform.BLL.MappingProfiles;
using Microsoft.EntityFrameworkCore;
using OnlineClassifiedsPlatform.BLL.Exceptions;

namespace OnlineClassifiedsPlatform.Tests.UnitTests.BLLTests
{
    [TestFixture]
    public class AccountServicesTest
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

        #region Registration Tests
        [Test, Order(0)]
        public async Task RegisterUserAsync_RegisteringUser_ReturnsTrue()
        {
            var userId = await _accService.RegisterUserAsync(_userDTO);
            Assert.IsTrue(userId != 0);
        }
        #endregion

        #region Veryfying Credentials Tests
        [Test, Order(2)]
        public async Task VerifyCredentialsAsync_VeryfyingExistingCredentials_ReturnsTrue()
        {
            await _accService.RegisterUserAsync(_userDTO);
            var loginResult = await _accService.VerifyCredentialsAsync(_userDTO.Username, _userDTO.Password);
            Assert.IsTrue(loginResult);
        }

        [Test, Order(3)]
        public async Task VerifyCredentialsAsync_VeryfyingNotExistingCredentials_ReturnsFalse()
        {
            await _accService.RegisterUserAsync(_userDTO);
            _userDTO.Password = "ANOTHERPASS";
            var loginResult = await _accService.VerifyCredentialsAsync(_userDTO.Username, _userDTO.Password);
            Assert.IsFalse(loginResult);
        }
        #endregion
    }
}