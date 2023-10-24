using OnlineClassifiedsPlatform.BLL.DTO;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.BLL.Interfaces
{
    public interface IAzureStorageService
    {
        Task<long> AddImageMetadataAsync(ImageMetadataDTO dto);
        Task<long> AddNewAzureBlobTypeAsync(AzureBlobTypeDTO dto);
        Task<long> AddNewAzureStorageAccountAsync(AzureStorageAccountDTO dto);
        Task<long> AddNewBlobFileAsync(AzureBlobFileDTO dto);
        Task<bool> IsContainerNameExistAsync(string containerName);
        Task<bool> IsAccountExistAsync(string accountName);
        Task<bool> IsBlobTypeExistAsync(string blobType);
        Task<AzureBlobDTO> GetAzureBlobAsync(string ContainerName);
        Task<AzureBlobTypeDTO> GetAzureBlobTypeAsync(string BlobType);
        Task<ImageMetadataDTO> GetImageMetadataAsync(long Id);
        Task<AzureStorageAccountDTO> GetAzureStorageAccountAsync(string StorageAccount);
        Task<AzureBlobFileDTO> GetAzureBlobFileByNameAsync(string fileName);
        Task<long> GetAzureBlobTypeIdAsync(string BlobType);
        Task<long> GetAzureBlobIdAsync(string ContainerName);
        Task<bool> IsAllowedFileTypeAsync(string containerName, string fileType);
    }
}
