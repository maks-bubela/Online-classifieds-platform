using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.Models
{
    public class GoodsCreateModel
    {
        /// <summary>
        /// Category of goods
        /// </summary>
        public string GoodsCategoryName { get; set; }

        /// <summary>
        /// Name of goods
        /// </summary>
        public string GoodsName { set; get; }

        /// <summary>
        /// Description of goods
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Goods price
        /// </summary>
        public long Price { get; set; }
    }
}
