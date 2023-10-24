using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineClassifiedsPlatform.BLL.DTO;
using OnlineClassifiedsPlatform.BLL.Interfaces;
using OnlineClassifiedsPlatform.BLL.Services;
using OnlineClassifiedsPlatform.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.Controllers
{
    [Route("api/goods")]
    public class GoodsController : Controller
    {
        #region Constants
        private const string InvalidModel = "Invalid input model.";
        private const long ID_NOT_FOUND = 0;
        private const string GoodsSuccesUpdate = "Goods updated successfully";
        private const string GoodsNotUpdate = "Goods not updated";
        private const string GoodsSuccesDelete = "Goods deleted successfully";
        private const string GoodsNotDelete = "Goods not deleted";
        #endregion

        #region Services
        private readonly IMapper _mapper;
        private readonly IGoodsService _goodsService;
        private readonly IUserService _userService;
        #endregion

        public GoodsController(IGoodsService goodsService ,IUserService userService, IMapper mapper)
        {
            _goodsService = goodsService ?? throw new ArgumentNullException(nameof(IGoodsService));
            _userService = userService ?? throw new ArgumentNullException(nameof(IUserService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(IMapper));
        }

        /// <summary>
        /// Create a goods and returns created goods if add to database is successful
        /// </summary>
        /// <param name="goods">Goods information for creation</param>
        /// <returns></returns>
        /// <response code="200"> Goods is created </response>
        /// <response code="409"> Goods creation is failed </response>
        /// <response code="400">Model not valid</response>    
        /// <response code="203"> Not authorize</response>  
        [Authorize ,HttpPost]
        [Route("add")]
        public async Task<IActionResult> CreateGoodsAsync([FromBody] GoodsCreateModel goods)
        {
            var userId = User.Identity.GetUserId<long>();
            if (!await _userService.UserExistsAsync(userId))
                return StatusCode(StatusCodes.Status203NonAuthoritative);

            if (ModelState.IsValid)
            {
                var goodsDTO = _mapper.Map<GoodsDTO>(goods);
                goodsDTO.UserId = userId;

                var goodsId = await _goodsService.AddGoodsAsync(goodsDTO);
                if (goodsId != 0)
                {
                    var response = new
                    {
                        GoodsCategoryName = goodsDTO.GoodsCategoryName,
                        GoodsName = goodsDTO.GoodsName,
                        Price = goodsDTO.Price,
                        Description = goodsDTO.Description

                    };
                    return Json(response);
                }
                return Conflict();
            }
            return BadRequest(new { errorText = InvalidModel});
        }

        /// <summary>
        /// Gets authenticated goods info (name, price and description, owner).
        /// </summary>
        /// <returns></returns>
        /// <response code="200"> Info was found </response>>
        /// <response code="500"> Something is wrong on server </response>>
        [Authorize, HttpPost]
        [Route("goods/info")]
        public async Task<IActionResult> GetGoodsAsync(long goodsId)
        {
            var goodsDTO = await _goodsService.GetGoodsAsync(goodsId);
            if (goodsDTO == null) return StatusCode(500);

            var goodsInfo = _mapper.Map<GoodsInfoModel>(goodsDTO);
            return Json(goodsInfo);
        }

        /// <summary>
        /// Update goods info (name, price and description, available status).
        /// </summary>
        /// <returns></returns>
        /// <response code="200"> Info was updated </response>>
        /// <response code="400"> Model not valid</response>    
        /// <response code="409"> Goods updating is failed </response>
        /// <response code="500"> Something is wrong on server </response>>
        [Authorize, HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateGoodsAsync([FromBody, Required] GoodsUpdateModule goodsUpdate, long goodsUpdateId)
        {
            if (ModelState.IsValid)
            { 
                var goodsDTO =  _mapper.Map<GoodsDTO>(goodsUpdate);
                if (goodsDTO == null) return StatusCode(500);
                var userId = User.Identity.GetUserId<long>();
                goodsDTO.UserId = userId;

                var updateStatus = await _goodsService.UpdateGoodsByIdAsync(goodsUpdateId, goodsDTO);
                if (updateStatus) return Ok(new { message = GoodsSuccesUpdate });
                else return Conflict(new { errorText = GoodsNotUpdate });
            }
            return BadRequest(new { errorText = InvalidModel });
        }

        /// <summary>
        /// Delete goods.
        /// </summary>
        /// <returns></returns>
        /// <response code="200"> Goods was deleted </response>>
        /// <response code="400"> goodsDeleteId not valid</response>    
        /// <response code="409"> Goods delete is failed </response>
        /// <response code="500"> Something is wrong on server </response>>
        [Authorize, HttpPost]
        [Route("delete")]
        public async Task<IActionResult> DeleteGoodsAsync(long goodsDeleteId)
        {
            if (goodsDeleteId == ID_NOT_FOUND) return BadRequest();
            var userId = User.Identity.GetUserId<long>();

            var deleteStatus = await _goodsService.DeleteGoodsByIdAsync(goodsDeleteId, userId);
            if (deleteStatus) return Ok(new { message = GoodsSuccesDelete });
            else return Conflict(new { errorText = GoodsNotDelete });
        }

    }
}
