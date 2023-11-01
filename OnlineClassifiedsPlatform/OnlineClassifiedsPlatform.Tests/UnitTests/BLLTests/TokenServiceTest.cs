using Microsoft.EntityFrameworkCore;
using OnlineClassifiedsPlatform.BLL.Enums;
using OnlineClassifiedsPlatform.BLL.Interfaces;
using OnlineClassifiedsPlatform.BLL.Services;
using OnlineClassifiedsPlatform.DAL.Context;
using NUnit.Framework;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.Tests.UnitTests.BLLTests
{
    [TestFixture]
    class TokenServiceTest
    {
        #region Services
        private ITokenService _tokenService;
        private OnlineClassifiedsPlatformContext _context;
        #endregion

        #region Constants
        const string MintyIssueTrackerDBTest = "MintyIssueTrackerDBTest";
        const EnvirementTypes _envirementTypes = EnvirementTypes.Testing;
        #endregion

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<OnlineClassifiedsPlatformContext>();
            optionsBuilder.UseInMemoryDatabase(MintyIssueTrackerDBTest);
            _context = new OnlineClassifiedsPlatformContext(optionsBuilder.Options);
            _tokenService = new TokenService(_context);
        }
        #region GetMethodTest
        [Test]
        public async Task GetTokenSettingsAsync__FoundData_NotNullResult()
        {
            var data = await _tokenService.GetTokenSettingsAsync(_envirementTypes);
            Assert.NotNull(data);
        }
        #endregion
    }
}
