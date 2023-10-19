using System;

namespace OnlineClassifiedsPlatform.AzureStorage.Exceptions
{
    class BlobClientException : Exception
    {
        private const string BlobClientNotExist = "Blob client don`t exist!";

        public BlobClientException()
        : base(BlobClientNotExist)
        { }
    }
}
