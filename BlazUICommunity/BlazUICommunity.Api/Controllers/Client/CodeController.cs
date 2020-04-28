using Blazui.Community.Api.Service;
using Blazui.Community.Enums;
using Blazui.Community.Model.Models;
using Blazui.Community.Repository;
using Blazui.Community.Response;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace Blazui.Community.Api.Controllers.Client
{
    /// <summary>
    ///
    /// </summary>
    [Route("api/client/[controller]")]
    [ApiController]
    [SwaggerTag(description: "验证码相关")]
    public class CodeController : ControllerBase
    {
        /// <summary>
        ///
        /// </summary>
        private BZVerifyCodeRepository _bZVerifyCodeRepository;

        private readonly ICodeService _codeService;
        private readonly IMessageService _messageService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="bZVerifyCodeRepository"></param>
        /// <param name="codeService"></param>
        /// <param name="messageService"></param>
        public CodeController(BZVerifyCodeRepository bZVerifyCodeRepository, ICodeService codeService, IMessageService messageService)
        {
            _codeService = codeService;
            _messageService = messageService;
            _bZVerifyCodeRepository = bZVerifyCodeRepository;
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet("SendVerifyCode/{VerifyCodeType}/{userId}/{target}")]
        public async Task<IActionResult> SendVerifyCode(int VerifyCodeType, string userId, string target)
        {
            return await SendVerifyCode((EmailType)VerifyCodeType, userId, target);
        }

        private async Task<IActionResult> SendVerifyCode(EmailType verifyCodeType, string userId, string Target)
        {
            var code = _codeService.GenerateNumberCode(4);

            BzVerifyCodeModel bzVerifyCodeModel = CreateModel(verifyCodeType, userId, code);
            var result = await _bZVerifyCodeRepository.InsertAsync(bzVerifyCodeModel);
            if (!string.IsNullOrEmpty(result.Entity.Id))
            {
                bool sendResult = await Send(verifyCodeType, Target, code);
                if (sendResult)
                    return Ok(bzVerifyCodeModel.VerifyCode);
                else
                {
                    await _bZVerifyCodeRepository.ChangeStateByIdAsync(result.Entity.Id, -1, "");
                    return new BadRequestResponse("发送验证码失败");
                }
            }
            else
                return new BadRequestResponse("生成验证码失败");
        }

        private async Task<bool> Send(EmailType verifyCodeType, string Target, string code)
        {
            bool sendResult;
            switch (verifyCodeType)
            {
                case EmailType.EmailLogin:
                case EmailType.EmailChangePassword:
                case EmailType.EmailRetrievePassword:
                case EmailType.EmailBind:
                    sendResult = await _messageService.SendVerifyCodeForBindEmailAsync(Target, code);
                    break;

                case EmailType.MobileLogin:
                case EmailType.MobileBind:
                case EmailType.MobileRetrievePassword:
                case EmailType.MobileChangePassword:
                    sendResult = await _messageService.SendVerifyCodeForChangePasswordAsync(Target, code);
                    break;

                default:
                    throw new NotSupportedException();
            }

            return sendResult;
        }

        private static BzVerifyCodeModel CreateModel(EmailType verifyCodeType, string userId, string code)
        {
            return new BzVerifyCodeModel()
            {
                VerifyCode = code,
                UserId = userId,
                VerifyType = (int)verifyCodeType,
                ExpireTime = DateTime.Now.AddSeconds(120),
                CreateDate = DateTime.Now,
                CreatorId = Guid.Empty.ToString(),
                LastModifierId = Guid.Empty.ToString(),
                LastModifyDate = DateTime.Now
            };
        }

        /// <summary>
        /// 验证短信验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet("VerifyCode/{userId}/{CodeType}/{Code}")]
        public async Task<IActionResult> VerifyCode(string userId, int CodeType, string Code)
        {
            var result = await _bZVerifyCodeRepository.GetFirstOrDefaultAsync(p => p.VerifyCode == Code && p.UserId == userId && p.VerifyType == CodeType);
            if (result != null && !result.IsExpired)
                return Ok(result);
            return new BadRequestResponse("验证码已过期");
        }
    }
}