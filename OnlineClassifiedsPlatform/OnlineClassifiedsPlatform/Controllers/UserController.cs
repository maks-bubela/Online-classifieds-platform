using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineClassifiedsPlatform.BLL.Interfaces;
using OnlineClassifiedsPlatform.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets authenticated user info (role, fullname and username).
        /// </summary>
        /// <returns></returns>
        /// <response code="200"> Info was found </response>>
        /// <response code="500"> Something is wrong on server </response>>
        [HttpGet, Route("info")]
        [Authorize]
        public async Task<IActionResult> GetInfoAsync()
        {
            var userId = User.Identity.GetUserId<long>();
            var userDTO = await _userService.GetUserByIdAsync(userId);

            if (userDTO != null)
            {
                var response = new
                {
                    Username = userDTO.Username,
                    Firstname = userDTO.Firstname,
                    Lastname = userDTO.Lastname,
                    Role = userDTO.RoleName
                };
                return Json(response);
            }
            return StatusCode(500);
        }

        /// <summary>
        /// Gets list of all users.
        /// </summary>
        /// <returns></returns>
        /// <response code="200"> Info was found </response>>
        /// <response code="500"> Something is wrong on server </response>>
        [HttpGet, Route("list")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUsersListAsync()
        {
            var userDTOList = await _userService.GetUsersListAsync();
            if (userDTOList == null) return StatusCode(500);

            var userInfoList = _mapper.Map<List<UserInfoModel>>(userDTOList);
            return Json(userInfoList);
        }

        /// <summary>
        /// Performs soft deletion in data base
        /// </summary>
        /// <param name="userModel">Username and password to remove</param>
        /// <returns></returns>
        /// <response code="200"> User is removed </response>>
        /// <response code="400"> Invalid model </response>>
        /// <response code="403"> User isn't authenticated or doesn't have access </response>>
        /// <response code="409"> Removal is failed </response>>
        [HttpDelete, Route("remove")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RemoveAsync([FromQuery] long id)
        {
            if (ModelState.IsValid)
            {
                if (await _userService.SoftDeleteAsync(id))
                    return Ok();
                return Conflict();
            }
            return BadRequest();
        }
    }
}
