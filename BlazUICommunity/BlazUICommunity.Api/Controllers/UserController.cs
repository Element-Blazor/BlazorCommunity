using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using BlazUICommunity.DTO;
using BlazUICommunity.Model.Models;
using BlazUICommunity.Repository;
using BlazUICommunity.Request;
using BlazUICommunity.Utility.Extensions;
using BlazUICommunity.Utility.Response;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace BlazUICommunity.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[Controller]")]
    [ApiController]
    [SwaggerTag(description:"用户相关")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<BZUserModel> _userRepository;
        private readonly IMapper _mapper;
        private readonly BZUserRepository _bZUserRepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public UserController(IUnitOfWork unitOfWork ,
            IMapper mapper,
            BZUserRepository bZUserRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = unitOfWork.GetRepository<BZUserModel>(true);
            _bZUserRepository = bZUserRepository;
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
        [HttpDelete("Delete/{Id}")]
        public IActionResult Delete([FromRoute] int Id)
        {
            _userRepository.Delete(Id);
            return Ok();
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <returns></returns>
        [HttpPut("Update/{Id}")]
        public IActionResult Update([FromBody] BZUserDto Dto , [FromRoute] int Id)
        {
            if ( Id < 1 )
                return new BadRequestResponse("id is error");
            var user = _mapper.Map<BZUserModel>(Dto);
            user.Id = Id;

            //_userRepository.Update(user);
            _userRepository.UpdateSpecifiedField(user , p => p.Account);
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
            if ( res is null )
                return new NoContentResponse();
            return Ok(res);
        }
        /// <summary>
        /// 根据条件分页查询用户
        /// </summary>
        /// <returns></returns>
        [HttpPost("Query")]
        public async Task<IActionResult> Query([FromBody] UsersRequest Request = null)
        {
            IPagedList<BZUserModel> pagedList = null;
            var query = Request.CreateQueryExpression<BZUserModel , UsersRequest>();
            pagedList = query == null ? await _userRepository.GetPagedListAsync(Request.pageInfo.PageIndex - 1 , Request.pageInfo.PageSize) :
                       await _userRepository.GetPagedListAsync(query , o => o.OrderBy(p => p.Id) , null , Request.pageInfo.PageIndex - 1 , Request.pageInfo.PageSize);
            return Ok(pagedList);
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
            var ResultDtos = await _bZUserRepository.UserActive(DateTime.Now.AddDays(beforeDays) , DateTime.Now);
            if ( ResultDtos is null || !ResultDtos.Any() )
                return new NoContentResponse();
            return Ok(ResultDtos);
        }

        /// <summary>
        /// 我的主贴
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet("Topic/{userId}/{pageSize}/{pageIndex}/{title}")]
        public async Task<IActionResult> QueryTopic(int userId ,
            [SwaggerParameter(Required = false)] int pageSize = 20 ,
            [SwaggerParameter(Required = false)]int pageIndex = 1 ,
            [SwaggerParameter(Required = false)]string title = "")
        {
            if ( userId < 1 )
                return new BadRequestResponse(" user id  error");

            var repo = _unitOfWork.GetRepository<BZTopicModel>(true);
            Expression<Func<BZTopicModel , bool>> predicate = p => p.UserId == userId;
            if ( !string.IsNullOrWhiteSpace(title) )
                predicate.And(p => p.Title == title);
            var listData = await repo.GetPagedListAsync(predicate , o => o.OrderByDescending(p => p.PublishTime) , null , pageIndex - 1 , pageSize);
            if ( listData.TotalCount > 0 )
            {
                var resultData = _mapper.Map<List<BZTopicDto>>(listData?.Items);
                return Ok(resultData);
            }
            else
            {

                return new NoContentResponse();
            }
        }

        /// <summary>
        /// 我的回帖
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("Reply/{userId}/{pageSize}/{pageIndex}")]
        public async Task<IActionResult> QueryReply(int userId ,
            [SwaggerParameter(Required = false)] int pageSize = 20 ,
            [SwaggerParameter(Required = false)]int pageIndex = 1)
        {
            if ( userId < 1 )
                return new BadRequestResponse(" user id  error");

            var repo = _unitOfWork.GetRepository<BZReplyModel>(true);

            var listData = await repo.GetPagedListAsync(p => p.UserId == userId , o => o.OrderByDescending(p => p.PublishTime) , null , pageIndex - 1 , pageSize);
            if ( listData.TotalCount > 0 )
            {
                var resultData = _mapper.Map<List<BZReplyDto>>(listData?.Items);
                return Ok(resultData);
            }
            else
            {
                return new NoContentResponse();
            }
        }

        /// <summary>
        /// 我的关注
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("Follow/{userId}/{pageSize}/{pageIndex}")]
        public async Task<IActionResult> QueryFollow(int userId ,
            [SwaggerParameter(Required = false)] int pageSize = 20 ,
            [SwaggerParameter(Required = false)]int pageIndex = 1)
        {
            if ( userId < 1 )
                return new BadRequestResponse(" user id  error");

            var repo = _unitOfWork.GetRepository<BZFollowModel>(true);

            var listData = await repo.GetPagedListAsync(p => p.UserId == userId , o => o.OrderByDescending(p => p.FollowTime) , null , pageIndex - 1 , pageSize);
            if ( listData.TotalCount > 0 )
            {
                var resultData = _mapper.Map<List<BZFollowDto>>(listData?.Items);
                return Ok(resultData);
            }
            else
            {
                return new NoContentResponse();
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="change"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ChangPasswod([FromBody]ChangePwdModel change)
        {
            if ( change is null )
            {
                throw new ArgumentNullException(nameof(change));
            }
            var (success, message) = _bZUserRepository.ChangePwd(change.Account , change.OldPwd , change.NewPwd);
            return Ok(new { success , message });
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