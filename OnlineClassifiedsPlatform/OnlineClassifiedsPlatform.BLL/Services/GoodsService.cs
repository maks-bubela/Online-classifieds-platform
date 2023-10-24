using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineClassifiedsPlatform.BLL.DTO;
using OnlineClassifiedsPlatform.BLL.Exceptions;
using OnlineClassifiedsPlatform.BLL.Interfaces;
using OnlineClassifiedsPlatform.DAL.Context;
using OnlineClassifiedsPlatform.DAL.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.BLL.Services
{
    public class GoodsService : IGoodsService
    {
        private readonly IMapper _mapper;
        private readonly OnlineClassifiedsPlatformContext _ctx;

        private const long ID_NOT_FOUND = 0;

        public GoodsService(OnlineClassifiedsPlatformContext ctx, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        public async Task<long> AddGoodsAsync(GoodsDTO goodsDTO)
        {
            if (goodsDTO == null) throw new ArgumentException(nameof(goodsDTO));

            var goodsCategory = await _ctx.Set<GoodsCategory>().Where(x => x.CategoryName == goodsDTO.GoodsCategoryName)
                .SingleOrDefaultAsync() ?? throw new ArgumentNullException(nameof(goodsDTO.GoodsCategoryName));

            var goods = _mapper.Map<Goods>(goodsDTO);
            goods.GoodsCategoryId = goodsCategory.Id;
            goods.IsAvailable = true;
            _ctx.Set<Goods>().Add(goods);
            await _ctx.SaveChangesAsync();

            if (goods.Id > ID_NOT_FOUND)
                return goods.Id;
            throw new FailedAddToDatabaseException();
        }

        public async Task<long> AddGoodsPhotoAsync(long goodsId, Uri uri)
        {
            if (uri == null) throw new EntityArgumentNullException(nameof(uri));
            if (goodsId == 0) throw new ArgumentException(nameof(goodsId));

            var blobFile = await _ctx.Set<AzureBlobFile>()
                .Where(x => x.FileName == Path.GetFileName(uri.ToString()))
                .SingleOrDefaultAsync();
            if (blobFile == null) throw new EntityArgumentNullException(nameof(blobFile));

            var goodsPhoto = new GoodsPhoto
            {
                GoodsId = goodsId,
                GoodsImageId = blobFile.Id
            };
            _ctx.Set<GoodsPhoto>().Add(goodsPhoto);
            await _ctx.SaveChangesAsync();

            return goodsPhoto.Id;
        }

        public async Task<GoodsDTO> GetGoodsAsync(long goodsId)
        {
            if (goodsId == ID_NOT_FOUND) throw new ArgumentNullException(nameof(goodsId));

            var goods = await _ctx.Set<Goods>()
                .Include(x => x.GoodsPhotos)
                .SingleOrDefaultAsync(x => x.Id == goodsId);
            if (goods == null) throw new NullReferenceException(nameof(goods));

            var goodsDTO = _mapper.Map<GoodsDTO>(goods);
            List<long> goodsPhotoIds = goods.GoodsPhotos.Select(photo => photo.Id).ToList();
            goodsDTO.GoodsPhotosId = goodsPhotoIds;
            return goodsDTO;
        }

        public async Task<bool> UpdateGoodsByIdAsync(long goodsId, GoodsDTO goodsDTO)
        {
            if (goodsId == ID_NOT_FOUND) throw new ArgumentNullException(nameof(goodsId));
            if (goodsDTO == null) throw new ArgumentNullException(nameof(goodsDTO));
            var goods = await _ctx.Set<Goods>()
                .Include(x => x.GoodsPhotos)
                .SingleOrDefaultAsync(x => x.Id == goodsId);
            if (goods == null || goods.UserId != goodsDTO.UserId) return false;

            if (goodsDTO.GoodsName != null)
            {
                goods.GoodsName = goodsDTO.GoodsName;
            }

            if (goodsDTO.Description != null)
            {
                goods.Description = goodsDTO.Description;
            }

            if (goodsDTO.Price != 0)
            {
                goods.Price = goodsDTO.Price;
            }

            if (goodsDTO.IsAvailable != goods.IsAvailable)
            {
                goods.IsAvailable = goodsDTO.IsAvailable;
            }

            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteGoodsByIdAsync(long goodsId, long userId)
        {
            if (goodsId == ID_NOT_FOUND) throw new ArgumentNullException(nameof(goodsId));

            var goods = await _ctx.Set<Goods>()
                .Include(x => x.GoodsPhotos)
                .SingleOrDefaultAsync(x => x.Id == goodsId);
            if (goods == null || goods.UserId != userId) return false;

            _ctx.Set<Goods>().Remove(goods);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
