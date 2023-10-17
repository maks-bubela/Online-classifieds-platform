using OnlineClassifiedsPlatform.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineClassifiedsPlatform.DAL.Entities
{
    public class AzureBlob : IEntity
    {
        public long Id { get; set; }

        public string ContainerName { get; set; }

        public virtual ICollection<AzureBlobType> AllowAzureBlobTypes { get; set; }
            = new HashSet<AzureBlobType>();
        public long? AccountId { get; set; }
        public virtual AzureStorageAccount Account { get; set; }
        public virtual ICollection<AzureBlobFile> AzureBlobFiles { get; set; }
            = new HashSet<AzureBlobFile>();
    }
}
