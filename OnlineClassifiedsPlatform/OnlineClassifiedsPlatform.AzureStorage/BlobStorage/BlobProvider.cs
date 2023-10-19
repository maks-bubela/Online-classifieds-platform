using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using OnlineClassifiedsPlatform.AzureStorage.Exceptions;
using OnlineClassifiedsPlatform.AzureStorage.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.AzureStorage.BlobStorage
{
    public class BlobProvider : IBlobProvider
    {
        private readonly string _connectionString;
        private readonly BlobServiceClient _blobService;
        private readonly BlobClientOptions blobClientOptions = new BlobClientOptions();

        public BlobProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            blobClientOptions.Retry.MaxRetries = int.MaxValue;
            _blobService = new BlobServiceClient(_connectionString, blobClientOptions);
        }

        #region UploadMethods
        public async Task<Uri> UploadBlobAsync(BinaryData binaryData, string containerName, string fileName = null, string fileType = null)
        {
            if (binaryData == null) throw new ArgumentNullException(nameof(BinaryData));
            if (containerName == null) throw new ArgumentNullException(nameof(containerName));

            if (fileName == null)
            {
                if (fileType == null)
                    throw new ArgumentNullException(nameof(fileType));
                fileName = (GenerateFileName() + fileType);
            }
            var containerClient = _blobService.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(binaryData, true);
            return blobClient.Uri;
        }
        #endregion

        #region MoveMethods
        public async Task<Uri> MoveAsync(string containerName, Uri sourceUri)
        {
            if (sourceUri == null) throw new ArgumentNullException(nameof(Uri));
            if (containerName == null) throw new ArgumentNullException(nameof(containerName));

            var container = _blobService.GetBlobContainerClient(containerName);
            await container.CreateIfNotExistsAsync(PublicAccessType.Blob);
            var blockBlobClient = container.GetBlockBlobClient(await GetFileNameFromUri(sourceUri));

            var copyStatus = await blockBlobClient.StartCopyFromUriAsync(sourceUri);
            while (!copyStatus.HasCompleted)
            {
                await copyStatus.WaitForCompletionAsync();
            }
            return blockBlobClient.Uri;
        }
        #endregion

        #region GetMethods
        public async Task<byte[]> GetByteImageAsync(Uri sourceUri, string containerName)
        {
            if (sourceUri == null) throw new ArgumentNullException(nameof(Uri));
            if (containerName == null) throw new ArgumentNullException(nameof(containerName));

            var containerClient = _blobService.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(await GetFileNameFromUri(sourceUri));
            if (blobClient.Exists())
            {
                using (var ms = new MemoryStream())
                {
                    blobClient.DownloadTo(ms);
                    return ms.ToArray();
                }
            }
            throw new BlobClientException();
        }

        public async Task<string> GetFileNameFromUri(Uri sourceUri)
        {
            string filename = Path.GetFileName(sourceUri.AbsolutePath);
            return filename;
        }

        public Uri GetUri(string fileName, string containerName)
        {
            var uri = new Uri(_blobService.Uri.ToString() + '/' + containerName + '/' + fileName);
            return uri;
        }
        #endregion

        #region DeleteMethods
        public async Task<bool> DeleteFileAsync(string fileName, string containerName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (containerName == null) throw new ArgumentNullException(nameof(containerName));

            var container = _blobService.GetBlobContainerClient(containerName);
            await container.GetBlobClient(fileName).DeleteIfExistsAsync();
            return true;
        }

        public async Task<bool> DeleteFileAsync(Uri sourceUri, string containerName)
        {
            if (sourceUri == null) throw new ArgumentNullException(nameof(Uri));
            if (containerName == null) throw new ArgumentNullException(nameof(containerName));

            var container = _blobService.GetBlobContainerClient(containerName);
            await container.GetBlobClient(await GetFileNameFromUri(sourceUri)).DeleteIfExistsAsync();
            return true;
        }
        #endregion

        #region PrivateMethods
        private string GenerateFileName()
        {
            return Guid.NewGuid().ToString();
        }
        #endregion
    }
}
