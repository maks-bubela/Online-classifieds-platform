using OnlineClassifiedsPlatform.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineClassifiedsPlatform.DAL.Entities
{
    public class ImageMetadata : IEntity
    {
        public long Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public long? ImageId { get; set; }
        public virtual AzureBlobFile Image { get; set; }
    }
}
