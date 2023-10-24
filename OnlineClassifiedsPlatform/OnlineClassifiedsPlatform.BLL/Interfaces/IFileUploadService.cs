using Microsoft.AspNetCore.Http;
using System;

using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.BLL.Interfaces
{
    public interface IFileUploadService
    {
        Task<Uri> UploadFileAsync(string succesContainer, string tempContainer, IFormFile postedFile, long jobId = 0);
    }
}
