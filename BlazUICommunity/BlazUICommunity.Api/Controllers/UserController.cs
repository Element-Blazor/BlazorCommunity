using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using Blazui.Community.Api.Extensions;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Community.Repository;
using Blazui.Community.Request;
using Blazui.Community.Utility.Extensions;
using Blazui.Community.Utility.Response;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Blazui.Community.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        /// <param name="bZUserRepository"></param>
        public UserController(IUnitOfWork unitOfWork,
            IMapper mapper,
            BZUserRepository bZUserRepository, UserManager<BZUserModel> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = unitOfWork.GetRepository<BZUserModel>(true);
            _bZUserRepository = bZUserRepository;
            _userManager = userManager;
        }


        /// <summary>
        /// 新增用户
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] BZUserDto Dto)
        {
            var user = _mapper.Map<BZUserModel>(Dto);
            //await _userManager.CreateAsync(new IdentityUser(Dto.Account) { Email = Dto.Email , EmailConfirmed = true } , "1234");
            await _userRepository.InsertAsync(user);
            return Ok();
        }


        /// <summary>
        /// 根据ID删除用户
        /// </summary>
        /// <returns></returns>
        [HttpDelete("Delete/{Id}")]
        public IActionResult Delete([FromRoute] int Id)
        {
            _userRepository.Delete(Id);
            return Ok();
        }
        /// <summary>
        /// 冻结
        /// </summary>
        /// <returns></returns>
        [HttpGet("Frozen/{Id}")]
        public IActionResult Frozen([FromRoute] int Id)
        {
            _unitOfWork.CommitWithTransaction(() => {
                var user = _userRepository.GetFirstOrDefault(p => p.Id == Id);
                user.Status = -1;
                _userRepository.Update(user);
            });
            return Ok();
        }


        /// <summary>
        /// 解封
        /// </summary>
        /// <returns></returns>
        [HttpGet("UnFrozen/{Id}")]
        public IActionResult UnFrozen([FromRoute] int Id)
        {
            _unitOfWork.CommitWithTransaction(() => {
                var user = _userRepository.GetFirstOrDefault(p => p.Id == Id);
                user.Status =0;
                _userRepository.Update(user);
            });
            return Ok();
        }


        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        [HttpGet("ResetPassword/{Id}")]
        public async Task<IActionResult> ResetPassword([FromRoute] int Id)
        {
         var user=   await   _userManager.FindByIdAsync(Id.ToString());
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var newPassword = "888888";// new Random(DateTime.Now.Millisecond).Next(100000, 999999);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword.ToString());
            if (result.Succeeded)
                return Ok(newPassword);
            return new BadRequestResponse("重置密码失败");
        }
        /// <summary>
        /// 更新用户
        /// </summary>
        /// <returns></returns>
        [HttpPut("Update/{Id}")]
        public IActionResult Update([FromBody] BZUserDto Dto, [FromRoute] int Id)
        {
            if (Id < 1)
                return new BadRequestResponse("id is error");
            var user = _mapper.Map<BZUserModel>(Dto);
            user.Id = Id;
            _userRepository.Update(user);
            return Ok();
        }

        /// <summary>
        /// 根据Id查询用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query/{Id}")]
        public async Task<IActionResult> Query([FromRoute] int Id)
        {
            var res = await _userRepository.FindAsync(Id);
            if (res is null)
                return new NoContentResponse();
            return Ok(_mapper.Map<BZUserUIDto>(res));
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
                return new NoContentResponse();
            return Ok(_mapper.Map<BZUserUIDto>(res));
        }
    
        /// <summary>
        /// 根据条件分页查询用户
        /// </summary>
        /// <returns></returns>
        [HttpPost("Query")]
        public async Task<IActionResult> Query([FromBody] UsersRequest Request = null)
        {
            IPagedList<BZUserModel> pagedList = null;
            var query = Request.CreateQueryExpression<BZUserModel, UsersRequest>();
            if (query == null)
                query = p => true;
            //query = query.And(p => p.Status == 0);
            pagedList = await _userRepository.GetPagedListAsync(query, o => o.OrderBy(p => p.Id), null, Request.pageInfo.PageIndex - 1, Request.pageInfo.PageSize);
            var pagedatas = new PageDatas<BZUserUIDto>();
            if (pagedList != null && pagedList.Items.Any())
            {
                pagedatas = pagedList.ConvertToPageData<BZUserModel, BZUserUIDto>();
                pagedList.Items.ToList().ForEach(p =>
                {
                    var dto = _mapper.Map<BZUserUIDto>(p);
                    pagedatas.Items.Add(dto);
                });
            }
            if (pagedatas.TotalCount > 0)
                return Ok(pagedatas);
            else
                return new NoContentResponse( );
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
                return new NoContentResponse();
            return Ok(ResultDtos);
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="change"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ChangPasswod([FromBody]ChangePwdModel change)
        {
            if (change is null)
            {
                throw new ArgumentNullException(nameof(change));
            }
            var (success, message) = _bZUserRepository.ChangePwd(change.Account, change.OldPwd, change.NewPwd);
            return Ok(new { success, message });
        }


        //[HttpGet("TestBulk")]
        //public async Task<IActionResult> TestBulk()
        //{
        //    List<BZUserModel> bZUserModels = new List<BZUserModel>();
        //    for ( int i = 0; i < 20; i++ )
        //    {
        //        bZUserModels.Add(new BZUserModel() { 
        //         Account="12312"+i,
        //          Avatar="12312312"+i,
        //           Cypher="321312312",
        //            Email="eeee",
        //             Level=1,
        //              LastLoginAddr="1312312",
        //               LastLoginDate=DateTime.Now,
        //                LastLoginType=0,
        //                 Mobile="1231231",
        //                  NickName="321312312"+i,
        //                 Points=1,
        //                  RegisterDate=DateTime.Now,
        //                   Sex=1,
        //                    Signature="21312",
        //                     Status=0,  
        //        });
        //    }
        //    bZUserModels.Add(await _userRepository.FindAsync(1));
        //    _userRepository.BulkInsert(bZUserModels);
        //    //_userRepository.BulkInsert();
        //    return Ok();
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost("users")]
        //public async Task<IActionResult> AddUser([FromBody] BZUserModel user)
        //{
        // await   _userRepository.InsertAsync(user);
        //    return Ok();
        //}
    }
}