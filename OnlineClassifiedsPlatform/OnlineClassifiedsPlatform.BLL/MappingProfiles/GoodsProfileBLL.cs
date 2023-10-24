using AutoMapper;
using OnlineClassifiedsPlatform.BLL.DTO;
using OnlineClassifiedsPlatform.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineClassifiedsPlatform.BLL.MappingProfiles
{
    public class GoodsProfileBLL : Profile
    {
        public GoodsProfileBLL()
        {
            #region To DTO
            CreateMap<Goods, GoodsDTO>();
            #endregion

            #region from DTO
            CreateMap<GoodsDTO, Goods>();
            #endregion
        }
    }
}
