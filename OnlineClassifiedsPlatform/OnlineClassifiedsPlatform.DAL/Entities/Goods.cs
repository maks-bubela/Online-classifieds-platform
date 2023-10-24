
using OnlineClassifiedsPlatform.DAL.Interfaces;
using System.Collections.Generic;

namespace OnlineClassifiedsPlatform.DAL.Entities
{
    public class Goods : IEntity
    {
        public long Id { get; set; }
        public virtual GoodsCategory GoodsCategory { get; set; }
        public long GoodsCategoryId { get; set; }
        public string GoodsName { set; get; }
        public string Description { get; set; }
        public long Price { get; set; }
        public bool IsAvailable { get; set; }
        public virtual ICollection<GoodsPhoto> GoodsPhotos { get; private set; }
            = new HashSet<GoodsPhoto>();
        public virtual User User { get; set; }
        public long UserId { get; set; }
    }
}
