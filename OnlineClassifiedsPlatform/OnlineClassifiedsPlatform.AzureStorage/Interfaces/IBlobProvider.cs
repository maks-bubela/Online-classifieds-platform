using System;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.AzureStorage.Interfaces
{
    public interface IBlobProvider
    {
        Task<Uri> UploadBlobAsync(BinaryData binaryData, string containerName, string fileName = null, string fileType = null);
        Task<Uri> MoveAsync(string containerName, Uri sourceUri);
        Task<byte[]> GetByteImageAsync(Uri sourceUri, string containerName);
        Task<bool> DeleteFileAsync(string fileName, string containerName);
        Task<bool> DeleteFileAsync(Uri sourceUri, string containerName);
        Task<string> GetFileNameFromUri(Uri sourceUri);
        Uri GetUri(string fileName, string containerName);
    }
}
