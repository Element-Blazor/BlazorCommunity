using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Community.Repository;
using Blazui.Community.Utility;
using Blazui.Community.Utility.Filter;
using Blazui.Community.Utility.Jwt;
using IdentityModel;
using IdentityModel.Client;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Blazui.Community.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [SwaggerTag(description: "jwt鉴权")]
    public class AccessTokenController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly JwtService _jwtService;
        private readonly BZUserRepository _bZUserRepository;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="configuration"></param>
      /// <param name="cache"></param>
      /// <param name="unitOfWork"></param>
      /// <param name="jwtService"></param>
      /// <param name="bZUserRepository"></param>
        public AccessTokenController(IConfiguration configuration ,
            IMemoryCache cache , 
            IUnitOfWork unitOfWork ,
            JwtService jwtService,
            BZUserRepository bZUserRepository)
        {
            _configuration = configuration;
            _cache = cache;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _bZUserRepository = bZUserRepository;
        }



        /// <summary>
        /// 登录，获取后原来RefreshToken将失效。
        /// AccessToken有效时间30分钟
        /// RefreshToken有效时间60分钟
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Post([FromBody]LoginModel model)
        {
            var result = _bZUserRepository.Login(model.Account , model.Pw);
            if ( !result.success)
            {
                return Unauthorized(result);
            }

            var user = new SessionUser
            {
                Id = result.user.Id,
                Name = result.user.NickName ,
                Role = "user"
            };

            var refreshToken = Guid.NewGuid().ToString("N");
            var refreshTokenExpiredTime = DateTime.Now.AddMinutes(60);
            var cacheKey = $"RefreshToken:{refreshToken}";
            var cacheValue = JsonConvert.SerializeObject(user);

            _cache.Set(cacheKey , cacheValue ,TimeSpan.FromMinutes(60));

            return Ok(new
            {
                AccessToken = _jwtService.GetAccessToken(user) ,
                Code = 200 ,
                RefreshTokenExpired = refreshTokenExpiredTime.ConvertDateTimeToInt() ,
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
            if ( string.IsNullOrWhiteSpace(cacheStr) )
            {
                return Ok(new
                {
                    Code = 0 ,
                    Message = "Token不存在或已过期"
                });
            }

            var cacheUser = JsonConvert.DeserializeObject<SessionUser>(cacheStr);
            var userId = User.Claims.First(c => c.Type == JwtClaimTypes.Id);

            if ( userId == null || cacheUser.Id.ToString() != userId.Value )
            {
                return Ok(new
                {
                    Code = 0 ,
                    Message = "用户不匹配"
                });
            }

            var refreshToken = Guid.NewGuid().ToString("N");
            var cacheKey = $"RefreshToken:{refreshToken}";
            var refreshTokenExpiredTime = DateTime.Now.AddMinutes(60);

            _cache.Set(cacheKey , cacheStr,TimeSpan.FromMinutes(60));

            return Ok(new
            {
                AccessToken = _jwtService.GetAccessToken(cacheUser) ,
                Code = 200 ,
                RefreshTokenExpired = refreshTokenExpiredTime.ConvertDateTimeToInt() ,
                RefreshToken = refreshToken
            });
        }
    }
}