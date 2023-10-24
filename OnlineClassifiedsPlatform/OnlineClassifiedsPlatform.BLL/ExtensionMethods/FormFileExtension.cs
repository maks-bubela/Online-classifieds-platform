using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace OnlineClassifiedsPlatform.BLL.ExtensionMethods
{
    public static class FormFileExtension
    {
        public static BinaryData ToBinaryData(this IFormFile postedFile)
        {
            using (var binaryReader = new BinaryReader(postedFile.OpenReadStream()))
            {
                byte[] fileData = binaryReader.ReadBytes((int)postedFile.Length);
                var userFile = new BinaryData(fileData);
                return userFile;
            }
        }
    }
}
