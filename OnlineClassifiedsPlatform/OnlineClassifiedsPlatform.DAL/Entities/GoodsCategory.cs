using OnlineClassifiedsPlatform.DAL.Interfaces;
using System.Collections.Generic;

namespace OnlineClassifiedsPlatform.DAL.Entities
{
    public class GoodsCategory : IEntity
    {
        public long Id { get; set; }
        public string CategoryName { get; set; }
        public virtual ICollection<Goods> Goods { get; private set; } = new HashSet<Goods>();
    }
}
