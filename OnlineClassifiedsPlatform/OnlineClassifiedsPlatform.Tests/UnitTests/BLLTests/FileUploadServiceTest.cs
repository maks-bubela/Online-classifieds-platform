using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineClassifiedsPlatform.AzureStorage.BlobStorage;
using OnlineClassifiedsPlatform.AzureStorage.Interfaces;
using OnlineClassifiedsPlatform.BLL.Interfaces;
using OnlineClassifiedsPlatform.BLL.MappingProfiles;
using OnlineClassifiedsPlatform.BLL.Services;
using OnlineClassifiedsPlatform.DAL.Context;
using OnlineClassifiedsPlatform.MappingProfiles;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.Tests.UnitTests.BLLTests
{
    [TestFixture]
    class FileUploadServiceTest
    {
        #region Services
        private IAzureStorageService _azureStorageService;
        private IFileUploadService _fileUploadService;
        private IBlobProvider _blobProvider;
        private IMapper _mapper;
        private OnlineClassifiedsPlatformContext _context;
        #endregion

        #region Constants
        const string OnlineClassifiedsPlatformDBTest = "OnlineClassifiedsPlatformDBTest";
        const string BlobTempContainerImage = "temp-image";
        const string AzureStorage = "UseDevelopmentStorage=true";
        private const string BlobSuccessContainerImage = "success-image";
        const string FullPath = @"C:\WorkSpace\a.jpg";
        private const long Id = 1;
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
            });
            _mapper = new Mapper(config);
            _azureStorageService = new AzureStorageService(_mapper, _context);
            _blobProvider = new BlobProvider(AzureStorage);
            _fileUploadService = new FileUploadService(_blobProvider, _azureStorageService);
        }

        #region UploadTest
        [Test]
        public async Task UploadFileAsync_SuccesUpload_ReturnUri()
        {
            using var stream = new MemoryStream(File.ReadAllBytes(FullPath).ToArray());
            var formFile = new FormFile(stream, 0, stream.Length, "streamFile", FullPath.Split(@"\").Last());
            var result_1 = await _fileUploadService.UploadFileAsync(BlobSuccessContainerImage, BlobTempContainerImage, formFile);
            var result_2 = await _fileUploadService.UploadFileAsync(BlobSuccessContainerImage, BlobTempContainerImage, formFile, Id);
            Assert.IsNotEmpty(result_1.ToString());
            Assert.IsNotEmpty(result_2.ToString());
        }

        [Test]
        public void UploadFileAsync_NullSuccesContainer_ReturnException()
        {
            UploadFileTest(null, BlobTempContainerImage,FullPath);
        }

        [Test]
        public void UploadFileAsync_NullTempContainer_ReturnException()
        {
            UploadFileTest(BlobSuccessContainerImage, null, FullPath);
        }

        [Test]
        public void UploadFileAsync_NullFormFile_ReturnException()
        {
            UploadFileTest(BlobSuccessContainerImage, BlobTempContainerImage, null);
        }
        #endregion

        #region PrivateMethods
        private void UploadFileTest(string successContainer, string tempConatiner, string path)
        {
            IFormFile formFile = null;
            if (path != null)
            {
                using var stream = new MemoryStream(File.ReadAllBytes(path).ToArray());
                formFile = new FormFile(stream, 0, stream.Length, "streamFile", path.Split(@"\").Last());
            }
            Assert.ThrowsAsync<ArgumentNullException>(() => _fileUploadService.UploadFileAsync(successContainer, tempConatiner, formFile));
        }
        #endregion
    }
}
