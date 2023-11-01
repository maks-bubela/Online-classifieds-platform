using OnlineClassifiedsPlatform.AzureStorage.BlobStorage;
using OnlineClassifiedsPlatform.AzureStorage.Interfaces;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.Tests.UnitTests.BlobTest
{
    [TestFixture]
    class BlobProviderTest
    {
        #region Services
        private IBlobProvider _blobProvider;
        #endregion

        #region Constants
        const string FILE_NAME = "a.jpg";
        const string FULL_PATH = @"C:\WorkSpace\a.jpg";
        const string CONTAINER_NAME = "test-image";
        const string CONTAINER_FOR_COPY = "test-image-copy";
        const string AzureStorage = "UseDevelopmentStorage=true";
        #endregion

        [SetUp]
        public void SetUp()
        {
            _blobProvider = new BlobProvider(AzureStorage);
        }

        #region UploadBlobTest
        [Test]
        public async Task UploadBlobAsync_SuccesUpload_ReturnUri()
        {
            var URI = await UploadFileForTest();
            Assert.IsNotEmpty(URI.ToString());
        }

        [Test]
        public void UploadBlobAsync_NullBinaryData_ReturnException()
        {
            UploadFileForTest(null,CONTAINER_NAME,FILE_NAME);
        }

        [Test]
        public void UploadBlobAsync_NullContainer_ReturnException()
        {
            UploadFileForTest(FULL_PATH, null, FILE_NAME);
        }

        [Test]
        public void UploadBlobAsync_NullFileName_ReturnException()
        {
            UploadFileForTest(FULL_PATH, CONTAINER_NAME, null);
        }
        #endregion

        #region MoveTest
        [Test]
        public async Task MoveAsync_SuccesMove_ReturnUri()
        {
            var uploadUri = await UploadFileForTest();
            var resultUri = await _blobProvider.MoveAsync(CONTAINER_FOR_COPY, uploadUri);
            Assert.IsNotEmpty(resultUri.ToString());
        }

        [Test]
        public async Task MoveAsync_NullContainer_ReturnException()
        {
            var uploadUri = await UploadFileForTest();
            Assert.ThrowsAsync<ArgumentNullException>(() => _blobProvider.MoveAsync(null, uploadUri));
        }

        [Test]
        public void MoveAsync_NullUri_ReturnException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _blobProvider.MoveAsync(CONTAINER_FOR_COPY, null));
        }
        #endregion

        #region DeleteTest
        [Test]
        public async Task DeleteFileAsync_SuccesDeleteFileName_ReturnTrue()
        {
            var result = await _blobProvider.DeleteFileAsync(FILE_NAME, CONTAINER_NAME);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteFileAsync_SuccesDeleteUri_ReturnTrue()
        {
            var uploadUri = await UploadFileForTest();
            var result = await _blobProvider.DeleteFileAsync(uploadUri, CONTAINER_NAME);
            Assert.IsTrue(result);
            Assert.NotNull(uploadUri);
        }

        [Test]
        public void DeleteFileAsync_NullFileName_ReturnException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _blobProvider.DeleteFileAsync(fileName:null, CONTAINER_NAME));
        }

        [Test]
        public void DeleteFileAsync_NullContainerName_ReturnException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _blobProvider.DeleteFileAsync(FILE_NAME, null));
        }

        [Test]
        public void DeleteFileAsync_NullUri_ReturnException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _blobProvider.DeleteFileAsync(sourceUri:null, CONTAINER_NAME));
        }
        #endregion

        #region GetTest
        [Test]
        public async Task GetByteImageAsync_SuccesByte_ReturnByte()
        {
            var uploadUri = await UploadFileForTest();
            var result = await _blobProvider.GetByteImageAsync(uploadUri, CONTAINER_NAME);
            Assert.NotNull(result);
        }

        [Test]
        public void GetByteImageAsync_NullUri_ReturnException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _blobProvider.GetByteImageAsync(null, CONTAINER_NAME));
        }

        [Test]
        public async Task GetByteImageAsync_NullContainer_ReturnException()
        {
            var uploadUri = await UploadFileForTest();
            Assert.ThrowsAsync<ArgumentNullException>(() => _blobProvider.GetByteImageAsync(uploadUri, null));
        }
        #endregion

        #region PrivateMethods
        private async Task<Uri> UploadFileForTest()
        {
            BinaryData binaryData;
            using (FileStream file = new FileStream(FULL_PATH, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                binaryData = new BinaryData(bytes);
            }
            return (await _blobProvider.UploadBlobAsync(binaryData, CONTAINER_NAME, FILE_NAME));
        }

        private void UploadFileForTest(string path, string container, string fileName)
        {
            BinaryData binaryData = null;
            if (path != null)
            {
                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                    binaryData = new BinaryData(bytes);
                }
            }
            Assert.ThrowsAsync<ArgumentNullException>(() => _blobProvider.UploadBlobAsync(binaryData, container, fileName));
        }
        #endregion
    }
}
