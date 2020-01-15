using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using BlazUICommunity.Model.Models;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlazUICommunity.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<BZUserModel> _userRepository;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userRepository = unitOfWork.GetRepository<BZUserModel>(true);
        }


    

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("users/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetUsers([FromRoute] int pageIndex,[FromRoute] int pageSize)
        {
            var res = await _userRepository.GetPagedListAsync(pageIndex-1 , pageSize);
            return Ok(res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUser([FromRoute] int userId)
        {
            var res = await _userRepository.FindAsync(userId);
            return Ok(res);
        }

        /// <summary>
        /// 
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
        public IActionResult PatchUser([FromRoute] int userId,[FromBody] BZUserModel user)
        {
            _userRepository.Update(user);
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("users")]
        public async Task<IActionResult> AddUser([FromBody] BZUserModel user)
        {
         await   _userRepository.InsertAsync(user);
            return Ok();
        }
    }
}