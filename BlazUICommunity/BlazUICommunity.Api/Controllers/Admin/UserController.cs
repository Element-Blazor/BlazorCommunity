using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using Blazui.Community.Api.Extensions;
using Blazui.Community.Api.Service;
using Blazui.Community.DTO;
using Blazui.Community.DTO.Admin;
using Blazui.Community.Model.Models;
using Blazui.Community.Repository;
using Blazui.Community.Request;
using Blazui.Community.Utility.Extensions;
using Blazui.Community.Utility.Filter;
using Blazui.Community.Utility.Response;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Blazui.Community.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    //[HiddenApi]
    [Route("api/[Controller]")]
    [ApiController]
    [SwaggerTag(description: "用户相关")]
    [HttpCacheExpiration(CacheLocation = CacheLocation.Public)]
    [HttpCacheValidation(MustRevalidate = true)]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly BZUserRepository _bZUserRepository;
        private readonly UserManager<BZUserModel> _userManager;
        private readonly ICacheService _cacheService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        /// <param name="bZUserRepository"></param>
        /// <param name="userManager"></param>
        /// <param name="cacheService"></param>
        public UserController(IUnitOfWork unitOfWork,
            IMapper mapper,
            BZUserRepository bZUserRepository, UserManager<BZUserModel> userManager, ICacheService cacheService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _bZUserRepository = bZUserRepository;
            _userManager = userManager;
            _cacheService = cacheService;
        }



        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPut("Frozen/{Id}")]
        public async Task<IActionResult> Frozen([FromRoute] string Id)
        {
            var user = await _bZUserRepository.FindAsync(Id);
            if (user is null)
                return BadRequest();
            if (user.Status == -1)
                return Ok();
            user.Status = -1;
            _bZUserRepository.Update(user);
            _cacheService.Remove(nameof(BZUserModel));
            return Ok();
        }


        /// <summary>
        /// 解封
        /// </summary>
        /// <returns></returns>
        [HttpPut("UnFrozen/{Id}")]
        public async Task<IActionResult> UnFrozen([FromRoute] string Id)
        {
            var user = await _bZUserRepository.FindAsync(Id);
            if (user is null)
                return BadRequest();
            if (user.Status == 0)
                return Ok();
            user.Status = 0;
            _bZUserRepository.Update(user);
            _cacheService.Remove(nameof(BZUserModel));
            return Ok();
        }


        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        [HttpPut("ResetPassword/{Id}")]
        public async Task<IActionResult> ResetPassword([FromRoute] string Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var newPassword = new Random(DateTime.Now.Millisecond).Next(10000000, 99999999);//"88888888";// 
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword.ToString());
            if (result.Succeeded)
                return Ok(newPassword);
            return new BadRequestResponse(JsonConvert.SerializeObject(result.Errors));
        }

        /// <summary>
        /// 根据条件分页查询用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query")]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> Query([FromQuery] UsersRequestCondition Request = null)
        {
            IPagedList<BZUserModel> pagedList = null;
            var query = Request.CreateQueryExpression<BZUserModel, UsersRequestCondition>();
            if (query == null)
                query = p => true;
            pagedList = await _bZUserRepository.GetPagedListAsync(query, o => o.OrderBy(p => p.Id), null, Request.PageInfo.PageIndex - 1, Request.PageInfo.PageSize);
            if (pagedList != null && pagedList.TotalCount > 0)
                return Ok(pagedList.From(result => _mapper.Map<List<UserDisplayDto>>(result)));
            return new NoContentResponse();
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

    }
}