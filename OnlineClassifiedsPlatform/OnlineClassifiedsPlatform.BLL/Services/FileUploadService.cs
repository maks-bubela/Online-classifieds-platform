using Microsoft.AspNetCore.Http;
using OnlineClassifiedsPlatform.AzureStorage.Interfaces;
using OnlineClassifiedsPlatform.BLL.DTO;
using OnlineClassifiedsPlatform.BLL.ExtensionMethods;
using OnlineClassifiedsPlatform.BLL.Interfaces;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.BLL.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IBlobProvider _blobProvider;
        private readonly IAzureStorageService _azureStorageService;

        private const string BLOB_TEMP_CONTAINER_IMAGE = "temp-image";
        private const long ID_NOT_FOUND = 0;

        public FileUploadService(IBlobProvider blobProvider, IAzureStorageService azureStorageService)
        {
            _blobProvider = blobProvider ?? throw new ArgumentNullException(nameof(IBlobProvider));
            _azureStorageService = azureStorageService ?? throw new ArgumentNullException(nameof(IAzureStorageService));
        }

        public async Task<Uri> UploadFileAsync(string successContainer, string tempContainer,
            IFormFile postedFile, long jobId = ID_NOT_FOUND)
        {
            if (successContainer == null) throw new ArgumentNullException(nameof(successContainer));
            if (tempContainer == null) throw new ArgumentNullException(nameof(tempContainer));
            if (postedFile == null) throw new ArgumentNullException(nameof(postedFile));

            var uri = await _blobProvider.UploadBlobAsync(postedFile.ToBinaryData(), tempContainer, fileType: Path.GetExtension(postedFile.FileName));
            uri = await _blobProvider.MoveAsync(successContainer, uri) ?? throw new ArgumentNullException(nameof(Uri));
            var fileName = await _blobProvider.GetFileNameFromUri(uri);

            var blobTypeId = await _azureStorageService.GetAzureBlobTypeIdAsync(Path.GetExtension(postedFile.FileName));
            var blobContainerId = await _azureStorageService.GetAzureBlobIdAsync(successContainer);

            var azureBlobFileDTO = new AzureBlobFileDTO
            {
                FileName = fileName,
                BlobTypeId = blobTypeId,
                ContainerId = blobContainerId,
            };
            if (jobId > ID_NOT_FOUND)
                azureBlobFileDTO.JobId = jobId;
            var blobFileId = await _azureStorageService.AddNewBlobFileAsync(azureBlobFileDTO);
            await UploadMetadataAsync(blobFileId, postedFile);
            await _blobProvider.DeleteFileAsync(fileName, BLOB_TEMP_CONTAINER_IMAGE);
            return uri;
        }

        private async Task UploadMetadataAsync(long blobFileId, IFormFile file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            if (blobFileId <= ID_NOT_FOUND) throw new ArgumentNullException(nameof(blobFileId));

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                using (var image = Image.FromStream(memoryStream))
                {
                    await _azureStorageService.AddImageMetadataAsync(new ImageMetadataDTO()
                    {
                        Width = image.Width,
                        Height = image.Height,
                        ImageId = blobFileId
                    });
                }
            }
        }
    }
}
