using OnlineClassifiedsPlatform.BLL.Services;
using OnlineClassifiedsPlatform.AzureStorage.BlobStorage;
using OnlineClassifiedsPlatform.AzureStorage.Interfaces;
using OnlineClassifiedsPlatform.BLL.Interfaces;
using OnlineClassifiedsPlatform.DAL.Context;
using NUnit.Framework;
using System;
using System.IO;
using OnlineClassifiedsPlatform.BLL.DTO;
using System.Threading.Tasks;
using AutoMapper;
using OnlineClassifiedsPlatform.BLL.MappingProfiles;
using OnlineClassifiedsPlatform.MappingProfiles;
using OnlineClassifiedsPlatform.BLL.Cryptography;
using Microsoft.EntityFrameworkCore;
using Bogus;

namespace OnlineClassifiedsPlatform.Tests.Integrations
{
    [TestFixture]
    class BlobAndServicesTest
    {
        #region Services
        private IAzureStorageService _azureStorageService;
        private IAccountService _accountService;
        private IBlobProvider _blobProvider;
        private IMapper _mapper;
        private IPasswordProcessing _passwordProcessing;
        private OnlineClassifiedsPlatformContext _context;
        private IUserService _userService;
        #endregion

        #region Constants
        const string FileName = "a.jpg";
        const string FullPath = @"C:\WorkSpace\a.jpg";
        const string ContainerName = "temp-image";
        const string Role = "admin";
        private const long Id = 1;
        const string MintyIssueTrackerDBTest = "MintyIssueTrackerDBTest";
        const string AzureStorage = "UseDevelopmentStorage=true";
        #endregion

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<OnlineClassifiedsPlatformContext>();
            optionsBuilder.UseInMemoryDatabase(MintyIssueTrackerDBTest);
            _context = new OnlineClassifiedsPlatformContext(optionsBuilder.Options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new UserProfileBLL());
                cfg.AddProfile(new AzureStorageProfileBLL());
            /*cfg.AddProfile(new ImageProfile());
            cfg.AddProfile(new AzureStorageProfile());
            cfg.AddProfile(new JobProfile());
            cfg.AddProfile(new JobProfileBLL());*/
            });
            _passwordProcessing = new PasswordProcessing();
            _mapper = new Mapper(config);
            _userService = new UserService(_mapper, _context);
            _azureStorageService = new AzureStorageService(_mapper, _context);
            _accountService = new AccountService(_context, _mapper, _passwordProcessing, _userService);
            _blobProvider = new BlobProvider(AzureStorage);
        }
        
        [Test(Description = "Test")]
        public async Task shouldUploadFileAndAddToDbMetaData_SuccesUploadAndAdd_ReturnTrue()
        {
            var binData = GetFileForTest();
            var uri = await _blobProvider.UploadBlobAsync(binData, ContainerName, FileName);
            var resultAddDb = await AddBlobFileAsyncToDbForTests();
            Assert.NotNull(resultAddDb);
            Assert.NotNull(uri);
        }


        #region PrivateMethods
        private System.BinaryData GetFileForTest()
        {
            BinaryData binaryData;
            using (FileStream file = new FileStream(FullPath, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                binaryData = new BinaryData(bytes);
            }
            return binaryData;
        }

        private async Task<long> AddBlobFileAsyncToDbForTests()
        {
            var blobTypeId = await _azureStorageService.GetAzureBlobTypeIdAsync(Path.GetExtension(FileName));
            var containerId = await _azureStorageService.GetAzureBlobIdAsync(ContainerName);
            var azureBlobFileDTO = new AzureBlobFileDTO
            {
                FileName = FileName,
                BlobTypeId = blobTypeId,
                ContainerId = containerId
            };
            return await _azureStorageService.AddNewBlobFileAsync(azureBlobFileDTO);
        }
        #endregion
    }
}
