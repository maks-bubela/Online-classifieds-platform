using OnlineClassifiedsPlatform.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineClassifiedsPlatform.DAL.Entities
{
    public class AzureBlobFile : IEntity
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public long BlobTypeId { get; set; }
        public virtual AzureBlobType FileType { get; set; }
        public long ContainerId { get; set; }
        public virtual AzureBlob Container { get; set; }
        public virtual ImageMetadata ImageMetadata { get; set; }
        public virtual GoodsPhoto GoodsPhoto { get; set; }
    }
}
