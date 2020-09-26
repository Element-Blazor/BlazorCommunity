using AutoMapper;
using BlazorCommunity.Api.Service;
using BlazorCommunity.DTO;
using BlazorCommunity.DTO.App;
using BlazorCommunity.Model.Models;
using BlazorCommunity.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCommunity.Api.Controllers.Client
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
        private readonly UserManager<BZUserModel> userManager;
        private readonly RoleManager<IdentityRole<string>> roleManager;
        private readonly IMapper mapper;

        /// <summary>
        ///
        /// </summary>
        /// <param name="bZUserRepository"></param>
        /// <param name="cacheService"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="mapper"></param>
        public UserController(
            BZUserRepository bZUserRepository, 
            ICacheService cacheService,
            UserManager<BZUserModel> userManager,
            RoleManager<IdentityRole<string>> roleManager,IMapper mapper)
        {
            _bZUserRepository = bZUserRepository;
            _cacheService = cacheService;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
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


        [HttpGet("Current/{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _bZUserRepository.FindAsync(userId);
            return user is null ? (IActionResult)BadRequest() : Ok(mapper.Map<BZUserDto>(user));
        }

        [HttpGet("GetRoles/{userId}")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var user = await _bZUserRepository.FindAsync(userId);
            if (user is null)
                return BadRequest();
            var roles = await userManager.GetRolesAsync(user);
            if (roles is null)
                roles = new List<string>();
            return Ok(roles);
        }

        [HttpGet("GetRoleNameById/{roleId}")]
        public async Task<IActionResult> GetRoleNameById(string roleId)
        {

            var role =  roleManager.Roles.FirstOrDefault(p => p.Id == roleId);
            if (role != null)
                return Ok(role?.Name);
            else return BadRequest();
        }

        [HttpGet("IsUserInRole/{roleId}/{userId}")]
        public async Task<IActionResult> IsUserInRole(string roleId,string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var role = roleManager.Roles.FirstOrDefault(p => p.Id == roleId);
            var isinrole = await userManager.IsInRoleAsync(user, role.Name);
            return Ok(isinrole);
        }



        [HttpGet("FindUserByEmail/{email}")]
        public async Task<IActionResult> FindUserByEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user is null ? (IActionResult)BadRequest() : Ok(mapper.Map<BZUserDto>(user));
        }

        [Authorize]
        [HttpPatch("Update")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
        {
            var user = await userManager.FindByIdAsync(updateUserDto.Id);
            if (user != null)
            {
                user.Signature = updateUserDto.Signature;
                user.Sex = updateUserDto.Sex;
                user.NickName = updateUserDto.NickName;

                var updateResult = await userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPatch("UpdateEmail")]
        public async Task<IActionResult> UpdateEmail(UpdateUserEmailDto  updateUserEmailDto)
        {
            var user = await userManager.FindByIdAsync(updateUserEmailDto.UserId);
                if(user!=null)
            {
                user.Email = updateUserEmailDto.Email;
                var updateResult = await userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            return BadRequest();
         
        }



        [Authorize]
        [HttpPatch("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(UpdateUserPasswordDto  updateUserPasswordDto)
        {
            var user = await userManager.FindByIdAsync(updateUserPasswordDto.UserId);
            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var updateResult = await userManager.ResetPasswordAsync(user, token, updateUserPasswordDto.NewPassword);
                if (updateResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            return BadRequest();

        }



        [Authorize]
        [HttpPatch("UpdateUserAvator")]
        public async Task<IActionResult> UpdateUserAvator(UpdateUserAvatorDto  updateUserAvatorDto)
        {
            var user = await userManager.FindByIdAsync(updateUserAvatorDto.UserId);
            if (user != null)
            {
                user.Avator = updateUserAvatorDto.AvatorUrl;
                var updateResult = await userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// 获取主题帖的作者信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("TopicUser/{TopicId}")]
        public async Task<IActionResult> TopicUser(string TopicId)
        {
            HotUserDto userDto = new HotUserDto();
            var topic = (await _cacheService.GetTopicsAsync(p => p.Id == TopicId))?.FirstOrDefault();
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