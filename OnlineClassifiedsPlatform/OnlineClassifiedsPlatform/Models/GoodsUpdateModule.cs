using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.Models
{
    public class GoodsUpdateModule
    {
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

        /// <summary>
        /// Goods available status
        /// </summary>
        public bool IsAvailable { get; set; }
    }
}
