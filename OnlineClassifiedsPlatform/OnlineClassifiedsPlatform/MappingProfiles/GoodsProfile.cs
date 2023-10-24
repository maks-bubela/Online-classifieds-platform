using AutoMapper;
using OnlineClassifiedsPlatform.BLL.DTO;
using OnlineClassifiedsPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.MappingProfiles
{
    public class GoodsProfile : Profile
    {
        public GoodsProfile()
        {
            #region To Model
            CreateMap<GoodsDTO, GoodsCreateModel>();
            CreateMap<GoodsDTO, GoodsInfoModel>();
            CreateMap<GoodsDTO, GoodsUpdateModule>();
            #endregion

            #region To DTO
            CreateMap<GoodsCreateModel, GoodsDTO>();
            CreateMap<GoodsInfoModel, GoodsDTO>();
            CreateMap<GoodsUpdateModule, GoodsDTO>();
            #endregion
        }
    }
}
