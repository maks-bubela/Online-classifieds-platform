using OnlineClassifiedsPlatform.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineClassifiedsPlatform.DAL.Entities
{
    public class AzureStorageAccount : IEntity
    {
        public long Id { get; set; }
        public string StorageAccount { get; set; }
        public string AccountHost { get; set; }
        public virtual ICollection<AzureBlob> AzureBlobs { get; set; }
             = new HashSet<AzureBlob>();
    }
}
