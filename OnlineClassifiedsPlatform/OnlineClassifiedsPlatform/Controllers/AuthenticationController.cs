using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineClassifiedsPlatform.BLL.DTO;
using OnlineClassifiedsPlatform.BLL.Enums;
using OnlineClassifiedsPlatform.BLL.Interfaces;
using OnlineClassifiedsPlatform.Interfaces;
using OnlineClassifiedsPlatform.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineClassifiedsPlatform.Controllers
{
    [Route("api/authentication")]
    public class AuthenticationController : Controller
    {
        #region Constants
        private const string InvalidUserData = "Invalid username or password.";
        private const string InvalidModel = "Invalid input model.";
        #endregion

        #region Services
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IAuthOptions _authOptions;
        private readonly ITokenService _tokenService;
        private readonly EnvirementTypes _envirementType;
        private readonly IUserService _userService;
        #endregion

        public AuthenticationController(ITokenService tokenService, IAccountService accountService,
            IMapper mapper, IAuthOptions authOptions, IUserService userService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(IAccountService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(IMapper));
            _authOptions = authOptions ?? throw new ArgumentNullException(nameof(IAuthOptions));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(ITokenService));
            _envirementType = EnvirementTypes.Development;
            _userService = userService ?? throw new ArgumentNullException(nameof(IUserService));

        }

        /// <summary>
        /// Authenticates a user and returns generated token if authentication is successful
        /// </summary>
        /// <param name="user">User and login information to authenticate</param>
        /// <returns></returns>
        /// <response code="200"> User is authenticated </response>
        /// <response code="409"> Authentication is failed </response>
        /// <response code="204"> Not founded environment type </response>
        /// <response code="400">Model not valid</response>    
        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> TokenAsync([FromBody] UserLoginModel user)
        {
            if (ModelState.IsValid)
            {
                var verified = await _accountService.VerifyCredentialsAsync(user.Username, user.Password);
                if (!verified)
                    return Conflict(new { errorText = InvalidUserData });
                var identity = await GetIdentityAsync(user);
                var lifeTime = await _tokenService.GetTokenSettingsAsync(_envirementType);
                if (lifeTime == 0)
                    return NoContent();
                var tokenSettingsDto = new TokenSettingsDTO()
                {
                    Identity = identity,
                    LifeTime = lifeTime
                };
                var encodedJwt = _authOptions.GetSymmetricSecurityKey(tokenSettingsDto);
                var response = new
                {
                    access_token = encodedJwt
                };
                return Json(response);
            }
            return BadRequest(new { errorText = InvalidUserData });
        }

        [HttpGet]
        [Route("test")]
        public IActionResult Test()
        {
            var text = "Test text";
            return Ok(text);
        }

        /// <summary>
        /// Registration new user and add him into database
        /// </summary>
        /// <param name="userRegist">User and login information to authenticate</param>
        /// <param name="roleName">User role name</param>
        /// <returns></returns>
        /// <response code="200"> User is authenticated </response>
        /// <response code="409"> Authentication is failed </response>
        /// <response code="400"> Model not valid </response>    
        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> RegistrationAsync(
            [FromBody] UserRegistrationModel userRegist, [FromQuery] string roleName)
        {
            if (ModelState.IsValid && userRegist != null)
            {
                var userDto = _mapper.Map<UserDTO>(userRegist);
                userDto.RoleName = roleName;

                var userId = await _accountService.RegisterUserAsync(userDto);
                if (userId != 0)
                {
                    var response = new
                    {
                        Username = userDto.Username,
                        Firstname = userDto.Firstname,
                        Lastname = userDto.Lastname
                    };
                    return Json(response);
                }
                return Conflict();
            }
            return BadRequest(new { errorText = InvalidModel });
        }

        private async Task<ClaimsIdentity> GetIdentityAsync(UserLoginModel userModel)
        {
            var verified = await _accountService.VerifyCredentialsAsync(userModel.Username, userModel.Password);
            if (verified)
            {
                var userDTO = await _userService.GetUserByUsernameAsync(userModel.Username);
                if (userDTO != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, userDTO.Username),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, userDTO.RoleName),
                        new Claim(ClaimTypes.NameIdentifier, userDTO.Id.ToString())
                    };
                    ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                    return claimsIdentity;
                }
            }
            return null;
        }
    }
}
