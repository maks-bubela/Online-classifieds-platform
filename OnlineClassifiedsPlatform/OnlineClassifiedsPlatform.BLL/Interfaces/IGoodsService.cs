using OnlineClassifiedsPlatform.BLL.DTO;
using OnlineClassifiedsPlatform.DAL.Entities;
using System;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.BLL.Interfaces
{
    public interface IGoodsService
    {
        Task<long> AddGoodsAsync(GoodsDTO goodsDTO);
        Task<long> AddGoodsPhotoAsync(long goodsId, Uri uri);
        Task<GoodsDTO> GetGoodsAsync(long goodsId);
        Task<bool> UpdateGoodsByIdAsync(long goodsId, GoodsDTO goodsDTO);
        Task<bool> DeleteGoodsByIdAsync(long goodsId, long userId);
        Task<Goods> BuyGoodsByIdAsync(long goodsId, long userId);
    }
}
