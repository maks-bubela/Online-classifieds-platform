using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineClassifiedsPlatform.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.Controllers
{
    [Route("api/image")]
    public class ImageController : Controller
    {

        #region Constants
        private const string QueueName = "cropimage";
        private const string BLOB_TEMP_CONTAINER_IMAGE = "temp-image";
        private const string BLOB_SUCCESS_CONTAINER_IMAGE = "success-image";
        private const string InvalidModel = "Invalid input model.";
        #endregion

        #region Services
        private readonly IMapper _mapper;
        private readonly IFileUploadService _fileUploadService;
        private readonly IGoodsService _goodsService;
        private readonly IAzureStorageService _azureStorageService;
        #endregion

        public ImageController(IGoodsService goodsService, IFileUploadService fileUploadService, IMapper mapper, 
            IAzureStorageService azureStorageService)
        {
            _goodsService = goodsService ?? throw new ArgumentNullException(nameof(IGoodsService));
            _fileUploadService = fileUploadService ?? throw new ArgumentNullException(nameof(IUserService));
            _azureStorageService = azureStorageService ?? throw new ArgumentNullException(nameof(IAzureStorageService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(IMapper));
        }

        /// <summary>
        /// Upload goods photo to blob and add record to table GoodsPhoto
        /// </summary>
        /// <returns></returns>
        /// <response code="201"> File upload </response>>
        /// <response code="415"> Not supported media type </response>>
        /// <response code="400"> Invalid empty file </response>>
        /// <response code="502"> File didn't upload </response>>
        /// <response code="500"> User not found </response>>
        /// <response code="404"> Not found blob container or user </response>>
        [Authorize, Route("upload/goods/photo")]
        [HttpPost]
        public async Task<IActionResult> UploadGoodsPhoto(IFormFile file, long goodsId)
        {

            if (!await _azureStorageService.IsContainerNameExistAsync(BLOB_TEMP_CONTAINER_IMAGE) &&
                !await _azureStorageService.IsContainerNameExistAsync(BLOB_SUCCESS_CONTAINER_IMAGE))
                return NotFound();

            if (file != null)
            {
                if (await _azureStorageService.IsAllowedFileTypeAsync(BLOB_TEMP_CONTAINER_IMAGE, Path.GetExtension(file.FileName)))
                {
                    var uri = await _fileUploadService.UploadFileAsync(BLOB_SUCCESS_CONTAINER_IMAGE, BLOB_TEMP_CONTAINER_IMAGE, file);
                    if (uri == null)
                        return StatusCode(StatusCodes.Status502BadGateway);

                    var goodsPhotoId = await _goodsService.AddGoodsPhotoAsync(goodsId, uri);
                    if (goodsPhotoId != 0)
                        return Created(uri, uri.ToString());
                }
                return StatusCode(StatusCodes.Status415UnsupportedMediaType);
            }
            return BadRequest();
        }
    }
}
