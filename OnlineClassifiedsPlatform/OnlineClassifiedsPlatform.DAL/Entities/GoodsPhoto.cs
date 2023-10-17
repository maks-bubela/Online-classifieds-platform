using OnlineClassifiedsPlatform.DAL.Interfaces;

namespace OnlineClassifiedsPlatform.DAL.Entities
{
    public class GoodsPhoto : IEntity
    {
        public long Id { get; set; }
        public virtual AzureBlobFile GoodsImage { get; set; }
        public long? GoodsImageId { get; set; }
        public virtual Goods Goods { get; set; }
        public long GoodsId { get; set; }
    }
}
