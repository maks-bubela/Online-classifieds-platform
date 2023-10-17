using OnlineClassifiedsPlatform.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineClassifiedsPlatform.DAL.Entities
{
    public class AzureBlobType : IEntity
    {
        public long Id { get; set; }
        public string BlobType { get; set; }
        public virtual ICollection<AzureBlob> AzureBlobs { get; set; }
             = new HashSet<AzureBlob>();
        public virtual ICollection<AzureBlobFile> AzureBlobFiles { get; set; }
            = new HashSet<AzureBlobFile>();
    }
}
