using Blazui.Community.Api.Service;
using Blazui.Community.DTO;
using Blazui.Community.Repository;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Api.Controllers.Client
{
    /// <summary>
    ///
    /// </summary>
    [Route("api/client/[controller]")]
    [ApiController]
    [SwaggerTag(description: "用户相关")]
    public class UserController : ControllerBase
    {
        private readonly BZUserRepository _bZUserRepository;
        private readonly ICacheService _cacheService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="bZUserRepository"></param>
        /// <param name="cacheService"></param>
        public UserController(
            BZUserRepository bZUserRepository, ICacheService cacheService)
        {
            _bZUserRepository = bZUserRepository;
            _cacheService = cacheService;
        }

        /// <summary>
        /// 活跃度
        /// </summary>
        /// <param name="ActiveType">1：月榜，2：周榜</param>
        /// <returns></returns>
        [HttpGet("Active")]
        public async Task<IActionResult> Active(int ActiveType = 1)
        {
            int beforeDays = ActiveType switch
            {
                1 => -30,
                2 => -7,
                _ => -7
            };
            var ResultDtos = await _bZUserRepository.UserActive(DateTime.Now.AddDays(beforeDays), DateTime.Now);
            if (ResultDtos is null || !ResultDtos.Any())
                return NoContent();
            return Ok(ResultDtos);
        }

        /// <summary>
        /// 活跃用户前10
        /// </summary>
        /// <returns></returns>
        [HttpGet("Hot")]
        public async Task<IActionResult> Hot()
        {
            return Ok(await _cacheService.GetHotUsersAsync());
        }
        
        /// <summary>
        /// 获取主题帖的作者信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("TopicUser/{TopicId}")]
        public async Task<IActionResult> TopicUser(string TopicId)
        {
            HotUserDto userDto = new HotUserDto();
            var topic =(await _cacheService.GetTopicsAsync(p => p.Id == TopicId))?.FirstOrDefault();
            if (topic != null)
            {
                var user = await _bZUserRepository.QueryTopicUser(topic.CreatorId);
                if (user != null)
                    userDto = user;
            }
            return Ok(userDto);
        }
    }
}