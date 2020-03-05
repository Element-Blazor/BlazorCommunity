using System;
using System.Linq;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using Blazui.Community.Api.Extensions;
using Blazui.Community.Api.Service;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Community.Repository;
using Blazui.Community.Request;
using Blazui.Community.Utility.Extensions;
using Blazui.Community.Utility.Filter;
using Blazui.Community.Utility.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Blazui.Community.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [HiddenApi]
    [Route("api/[Controller]")]
    [ApiController]
    [SwaggerTag(description: "用户相关")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<BZUserModel> _userRepository;
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
            _userRepository = unitOfWork.GetRepository<BZUserModel>(true);
            _bZUserRepository = bZUserRepository;
            _userManager = userManager;
            _cacheService = cacheService;
        }


        /// <summary>
        /// 新增用户
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] BZUserDto Dto)
        {
            var user = _mapper.Map<BZUserModel>(Dto);
            await _userRepository.InsertAsync(user);
            return Ok();
        }


        /// <summary>
        /// 根据ID删除用户
        /// </summary>
        /// <returns></returns>
        [HttpDelete("Delete/{Id}/{oprationId}")]
        public IActionResult Delete([FromRoute] string Id, string oprationId)
        {
            _userRepository.LogicDelete(Id, oprationId);
            return Ok();
        }
        /// <summary>
        /// 冻结
        /// </summary>
        /// <returns></returns>
        [HttpGet("Frozen/{Id}")]
        public IActionResult Frozen([FromRoute] string Id)
        {
            _unitOfWork.CommitWithTransaction(() =>
            {
                var user = _userRepository.GetFirstOrDefault(p => p.Id == Id);
                user.Status = -1;
                _userRepository.Update(user);
                _cacheService.Remove(nameof(BZUserModel));
            });
            return Ok();
        }


        /// <summary>
        /// 解封
        /// </summary>
        /// <returns></returns>
        [HttpGet("UnFrozen/{Id}")]
        public IActionResult UnFrozen([FromRoute] string Id)
        {
            _unitOfWork.CommitWithTransaction(() =>
            {
                var user = _userRepository.GetFirstOrDefault(p => p.Id == Id);
                user.Status = 0;
                _userRepository.Update(user);
                _cacheService.Remove(nameof(BZUserModel));
            });
            return Ok();
        }


        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        [HttpGet("ResetPassword/{Id}")]
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
        /// 更新用户
        /// </summary>
        /// <returns></returns>
        [HttpPut("Update/{Id}")]
        public IActionResult Update([FromBody] BZUserDto Dto)
        {
            if (!Guid.TryParse(Dto.Id, out _))
                return new BadRequestResponse("id is error");
            var user = _mapper.Map<BZUserModel>(Dto);

            _userRepository.Update(user);
            _cacheService.Remove(nameof(BZUserModel));
            return Ok();
        }

        /// <summary>
        /// 根据Id查询用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query/{Id}")]
        public async Task<IActionResult> Query([FromRoute] string Id)
        {
            var res = await _userRepository.FindAsync(Id);
            if (res is null)
                return NoContent();
            return Ok(_mapper.Map<BZUserDto>(res));
        }
        /// <summary>
        /// 根据Id查询用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("QueryByName/{UserName}")]
        public async Task<IActionResult> QueryByName([FromRoute] string UserName)
        {
            var res = await _userManager.FindByNameAsync(UserName);
            if (res is null)
                return NoContent();
            return Ok(_mapper.Map<BZUserDto>(res));
        }

        /// <summary>
        /// 根据条件分页查询用户
        /// </summary>
        /// <returns></returns>
        [HttpPost("Query")]
        public async Task<IActionResult> Query([FromBody] UsersRequestCondition Request = null)
        {
            IPagedList<BZUserModel> pagedList = null;
            var query = Request.CreateQueryExpression<BZUserModel, UsersRequestCondition>();
            if (query == null)
                query = p => true;
            //query = query.And(p => p.Status == 0);
            pagedList = await _userRepository.GetPagedListAsync(query, o => o.OrderBy(p => p.Id), null, Request.PageInfo.PageIndex - 1, Request.PageInfo.PageSize);
            var pagedatas = new PageDatas<BZUserDto>();
            if (pagedList != null && pagedList.Items.Any())
            {
                pagedatas = pagedList.ConvertToPageData<BZUserModel, BZUserDto>();
                pagedList.Items.ToList().ForEach(p =>
                {
                    var dto = _mapper.Map<BZUserDto>(p);
                    pagedatas.Items.Add(dto);
                });
            }
            if (pagedatas.TotalCount > 0)
                return Ok(pagedatas);
            else
                return NoContent();
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