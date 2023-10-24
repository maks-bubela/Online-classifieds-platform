using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineClassifiedsPlatform.BLL.DTO
{
    public class GoodsDTO
    {
        public string GoodsCategoryName { get; set; }
        public string GoodsCategoryId { get; set; }
        public string GoodsName { set; get; }
        public string Description { get; set; }
        public long Price { get; set; }
        public long UserId { get; set; }
        public bool IsAvailable { get; set; }
        public ICollection<long> GoodsPhotosId { get; set; }
            = new HashSet<long>();
    }
}
