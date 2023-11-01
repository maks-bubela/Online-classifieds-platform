using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineClassifiedsPlatform.BLL.DTO;
using OnlineClassifiedsPlatform.DAL.Context;
using OnlineClassifiedsPlatform.BLL.Services;
using OnlineClassifiedsPlatform.BLL.Interfaces;
using OnlineClassifiedsPlatform.MappingProfiles;
using OnlineClassifiedsPlatform.BLL.MappingProfiles;
using NUnit.Framework;
using System.Threading.Tasks;
using OnlineClassifiedsPlatform.BLL.Exceptions;
using System;

namespace OnlineClassifiedsPlatform.Tests.UnitTests.ServicesTest
{
    [TestFixture]
    class AzureStorageServiceTest
    {
        #region Services
        private IAzureStorageService _azureStorageService;
        private IMapper _mapper;
        private OnlineClassifiedsPlatformContext _context;
        #endregion

        #region Constants
        const string CONTAINER_NAME = "temp-image";
        const string BLOB_TYPE = ".jpg";
        const string NEW_BLOB_TYPE = ".new";
        const string ACCOUNT_NAME = "devstoreaccount1";
        const string FILE_NAME = "123.jpg";
        private const long ID = 1;
        const string OnlineClassifiedsPlatformDBTest = "OnlineClassifiedsPlatformDBTest";
        #endregion

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<OnlineClassifiedsPlatformContext>();
            optionsBuilder.UseInMemoryDatabase(OnlineClassifiedsPlatformDBTest);
            _context = new OnlineClassifiedsPlatformContext(optionsBuilder.Options);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new UserProfileBLL());
                cfg.AddProfile(new AzureStorageProfileBLL());
                /*cfg.AddProfile(new ImageProfile());
                cfg.AddProfile(new AzureStorageProfile());
                cfg.AddProfile(new JobProfileBLL());*/
            });
            _mapper = new Mapper(config);
            _azureStorageService = new AzureStorageService(_mapper, _context);
        }
        #region AddMethodsTest
        [Test]
        public async Task AddNewAzureStorageAccountAsync_CorrectData_ReturnOk()
        {
            const string NEW_ACCOUNT_NAME = "test-image";
            var azureStorageAccountDTO = new AzureStorageAccountDTO()
            {
                StorageAccount = NEW_ACCOUNT_NAME
            };
            var result = await _azureStorageService.AddNewAzureStorageAccountAsync(azureStorageAccountDTO);
            Assert.NotNull(result);
        }

        [Test]
        public void AddNewAzureStorageAccountAsync_NullDto_ReturnException()
        {
            Assert.ThrowsAsync<EntityArgumentNullException>(() => _azureStorageService.AddNewAzureStorageAccountAsync(null));
        }

        [Test]
        public async Task AddNewAzureBlobTypeAsync_CorrectData_ReturnOk()
        {
            var azureBlobTypeDTO = new AzureBlobTypeDTO()
            {
                BlobType = NEW_BLOB_TYPE
            };
            var result = await _azureStorageService.AddNewAzureBlobTypeAsync(azureBlobTypeDTO);
            Assert.NotNull(result);
        }

        [Test]
        public void AddNewAzureBlobTypeAsync_NullDto_ReturnException()
        {
            Assert.ThrowsAsync<EntityArgumentNullException>(() => _azureStorageService.AddNewAzureBlobTypeAsync(null));
        }

        [Test]
        public async Task AddNewBlobFileAsync_CorrectData_ReturnOk()
        {
            var blobFileDTO = new AzureBlobFileDTO()
            {
                FileName = FILE_NAME,
                BlobTypeId = ID,
                ContainerId = ID
            };
            var result = await _azureStorageService.AddNewBlobFileAsync(blobFileDTO);
            Assert.NotNull(result);
        }

        [Test]
        public void AddNewBlobFileAsync_NullDto_ReturnException()
        {
            Assert.ThrowsAsync<EntityArgumentNullException>(() => _azureStorageService.AddNewBlobFileAsync(null));
        }
        #endregion

        #region IsMethodTest
        [Test]
        public async Task IsContainerNameExistAsync__FoundData_ReturnTrue()
        {
            var result = await _azureStorageService.IsContainerNameExistAsync(CONTAINER_NAME);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsContainerNameExistAsync_NullContainer_ReturnException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _azureStorageService.IsContainerNameExistAsync(null));
        }

        [Test]
        public async Task IsAccountExistAsync__FoundData_ReturnTrue()
        {
            var result = await _azureStorageService.IsAccountExistAsync(ACCOUNT_NAME);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsAccountExistAsync_NullAccountName_ReturnException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _azureStorageService.IsAccountExistAsync(null));
        }

        [Test]
        public async Task IsBlobTypeExistAsync__FoundData_ReturnTrue()
        {
            var result = await _azureStorageService.IsBlobTypeExistAsync(BLOB_TYPE);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsBlobTypeExistAsync_NullBlobType_ReturnException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _azureStorageService.IsBlobTypeExistAsync(null));
        }

        [Test]
        public async Task IsAllowedFileTypeAsync__FoundData_ReturnTrue()
        {
            var result = await _azureStorageService.IsAllowedFileTypeAsync(CONTAINER_NAME, BLOB_TYPE);
            Assert.IsTrue(result);
        }
        #endregion

        #region GetMethodTest
        [Test]
        public async Task GetAzureBlobAsync_FoundData_NotNullResult()
        {
            var data = await _azureStorageService.GetAzureBlobAsync(CONTAINER_NAME);
            Assert.NotNull(data);
        }

        [Test]
        public void GetAzureBlobAsyncAsync_NullContainerName_ReturnException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _azureStorageService.GetAzureBlobAsync(null));
        }

        [Test]
        public async Task GetAzureBlobIdAsync_FoundData_NotNullResult()
        {
            var azureBlobId = await _azureStorageService.GetAzureBlobIdAsync(CONTAINER_NAME);
            Assert.NotNull(azureBlobId);
        }

        [Test]
        public void GetAzureBlobIdAsyncAsync_NullContainerName_ReturnException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _azureStorageService.GetAzureBlobIdAsync(null));
        }

        [Test]
        public async Task GetAzureBlobTypeAsync__FoundData_NotNullResult()
        {
            var blobTypeDTO = await _azureStorageService.GetAzureBlobTypeAsync(BLOB_TYPE);
            Assert.NotNull(blobTypeDTO);
        }

        [Test]
        public void GetAzureBlobTypeAsync_NullBlobType_ReturnException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _azureStorageService.GetAzureBlobTypeAsync(null));
        }

        [Test]
        public async Task GetAzureBlobTypeIdAsync__FoundData_NotNullResult()
        {
            var blobTypeId = await _azureStorageService.GetAzureBlobTypeIdAsync(BLOB_TYPE);
            Assert.NotNull(blobTypeId);
        }

        [Test]
        public void GetAzureBlobTypeIdAsync_NullBlobType_ReturnException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _azureStorageService.GetAzureBlobTypeIdAsync(null));
        }

        [Test]
        public async Task GetAzureStorageAccountAsync__FoundData_NotNullResult()
        {
            var data = await _azureStorageService.GetAzureStorageAccountAsync(ACCOUNT_NAME);
            Assert.NotNull(data);
        }

        [Test]
        public void GetAzureStorageAccountAsync_NullStorageAccount_ReturnException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _azureStorageService.GetAzureStorageAccountAsync(null));
        }
        #endregion
    }
}
