using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazui.Community.Model.Models;
using Blazui.Community.Repository;
using Blazui.Community.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Blazui.Community.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag(description: "验证码相关")]
    public class CodeController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private BZVerifyCodeRepository _bZVerifyCodeRepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bZVerifyCodeRepository"></param>
        public CodeController(BZVerifyCodeRepository bZVerifyCodeRepository)
        {
            _bZVerifyCodeRepository = bZVerifyCodeRepository;
        }

        /// <summary>
        /// 生成短信验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet("CreateCode/{userId}/{CodeType}/{Mobile}")]
        public async Task<IActionResult> CreateCode(int userId, int CodeType,string Mobile)
        {
            var code = RandomHelper.GenerateRandomCode(4);
            BzVerifyCodeModel bzVerifyCodeModel =
                new BzVerifyCodeModel() { VerifyCode = code, UserId = userId, VerifyType = CodeType, ExpireTime = DateTime.Now.AddSeconds(60) };
            var result = await _bZVerifyCodeRepository.InsertAsync(bzVerifyCodeModel);
            if (result.Entity.Id > 0)
            {
                return Ok(bzVerifyCodeModel.VerifyCode);
            }
            else
            {
                return BadRequest("生成验证码失败");
            }
        }


        /// <summary>
        /// 验证短信验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet("VerifyCode/{userId}/{CodeType}/{Code}")]
        public async Task<IActionResult> VerifyCode(int userId, int CodeType, string Code)
        {

            var result = await _bZVerifyCodeRepository.GetFirstOrDefaultAsync(p => p.VerifyCode == Code && p.UserId == userId && p.VerifyType == CodeType );
            if (result != null&& !result.IsExpired)
                return Ok(result);
            return BadRequest("验证失败");
        }
    }
}