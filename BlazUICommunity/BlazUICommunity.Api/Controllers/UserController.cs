using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using BlazUICommunity.DTO;
using BlazUICommunity.Model.Models;
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
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<BZUserModel> _userRepository;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public UserController(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _mapper = mapper;
               _unitOfWork = unitOfWork;
            _userRepository = unitOfWork.GetRepository<BZUserModel>(true);
        }

        /// <summary>
        /// 根据Id查询用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUser([FromRoute] int userId)
        {
            var res = await _userRepository.FindAsync(userId);
            return Ok(res);
        }

        /// <summary>
        /// 根据条件分页查询用户
        /// </summary>
        /// <returns></returns>
        [HttpPost("users")]
        public async Task<IActionResult> GetUsers([FromBody] UsersRequest usersRequest = null)
        {
            IPagedList<BZUserModel> pagedList = null;
            var query = usersRequest.CreateQueryExpression<BZUserModel , UsersRequest>();
            pagedList = query == null ? await _userRepository.GetPagedListAsync(usersRequest.pageInfo.PageIndex - 1 , usersRequest.pageInfo.PageSize) :
                       await _userRepository.GetPagedListAsync(query , o => o.OrderBy(p => p.Id) , null , usersRequest.pageInfo.PageIndex - 1 , usersRequest.pageInfo.PageSize);
            return Ok(pagedList);
        }

        /// <summary>
        /// 根据ID删除用户
        /// </summary>
        /// <returns></returns>
        [HttpDelete("users/{userId}")]
        public IActionResult DeleteUser([FromRoute] int userId)
        {
            _userRepository.Delete(userId);
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPatch("users/{userId}")]
        public IActionResult PatchUser([FromBody] BZUserDto userDto,[FromRoute] int userId)
        {
            if ( userId < 1 )
                return new BadRequestResponse("userid is error");
            var user = _mapper.Map<BZUserModel>(userDto);
            user.Id = userId;

            //_userRepository.Update(user);
            _userRepository.UpdateEntityField(user , p => p.Account);
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPut("users/{userId}")]
        public IActionResult UpdateUser([FromRoute] int userId , [FromBody] BZUserModel user)
        {

            _userRepository.Update(user);
            return Ok();
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