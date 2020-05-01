using Blazui.Community.Api.Jwt;
using Blazui.Community.DateTimeExtensions;
using Blazui.Community.Model.Models;
using Blazui.Community.Shared;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Api.Controllers
{
    /// <summary>
    ///
    /// </summary>
    //[Route("api/[controller]")]
    [ApiController]
    [SwaggerTag(description: "jwt鉴权")]
    public class TokenController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly JwtService _jwtService;
        private readonly UserManager<BZUserModel> _userManager;

        /// <summary>
        ///
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="jwtService"></param>
        /// <param name="userManager"></param>
        public TokenController(
            IMemoryCache cache,
            JwtService jwtService,
            UserManager<BZUserModel> userManager)
        {
            _cache = cache;
            _jwtService = jwtService;
            _userManager = userManager;
        }

        /// <summary>
        /// 登录，获取后原来RefreshToken将失效。
        /// AccessToken有效时间30分钟
        /// RefreshToken有效时间60分钟
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("token")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user is null)
            {
                return Unauthorized();
            }
            var checkPassword = user.PasswordHash == model.Password;// await _userManager.CheckPasswordAsync(user, model.Password);
            if (!checkPassword)
                return Unauthorized();
            var SessionUser = new SessionUser
            {
                Id = user.Id,
                Name = user.UserName,
                Role = "user"
            };

            var refreshToken = Guid.NewGuid().ToString("N");
            var refreshTokenExpiredTime = DateTime.Now.AddMinutes(60);
            var cacheKey = $"RefreshToken:{refreshToken}";
            var cacheValue = JsonConvert.SerializeObject(user);

            _cache.Set(cacheKey, cacheValue, TimeSpan.FromMinutes(60));

            return Ok(new
            {
                AccessToken = _jwtService.GetAccessToken(SessionUser),
                Code = 200,
                RefreshTokenExpired = refreshTokenExpiredTime.ConvertDateTimeToInt(),
                RefreshToken = refreshToken
            });
        }

        /// <summary>
        /// 刷新AccessToken
        /// </summary>
        /// <param name="token">刷新的请求 {"token": "refresh_token"}</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Refresh")]
        public IActionResult Refresh([FromQuery] string token)
        {
            var cacheStr = _cache.Get<string>($"RefreshToken:{token}");
            if (string.IsNullOrWhiteSpace(cacheStr))
            {
                return Ok(new
                {
                    Code = 0,
                    Message = "Token不存在或已过期"
                });
            }

            var cacheUser = JsonConvert.DeserializeObject<SessionUser>(cacheStr);
            var userId = User.Claims.First(c => c.Type == JwtClaimTypes.Id);

            if (userId == null || cacheUser.Id.ToString() != userId.Value)
            {
                return Ok(new
                {
                    Code = 0,
                    Message = "用户不匹配"
                });
            }

            var refreshToken = Guid.NewGuid().ToString("N");
            var cacheKey = $"RefreshToken:{refreshToken}";
            var refreshTokenExpiredTime = DateTime.Now.AddMinutes(60);

            _cache.Set(cacheKey, cacheStr, TimeSpan.FromMinutes(60));

            return Ok(new
            {
                AccessToken = _jwtService.GetAccessToken(cacheUser),
                Code = 200,
                RefreshTokenExpired = refreshTokenExpiredTime.ConvertDateTimeToInt(),
                RefreshToken = refreshToken
            });
        }
    }

   
}