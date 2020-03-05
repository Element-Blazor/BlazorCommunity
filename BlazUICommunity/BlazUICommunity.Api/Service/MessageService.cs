using Blazui.Community.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Blazui.Community.Api.Service
{
    public class MessageService : IMessageService
    {
        ISmtpClientService _smtpClientService;
        public MessageService(ISmtpClientService smtpClientService)
        {
            _smtpClientService = smtpClientService;
        }
        public Task<bool> SendEmail(string Email, string code, VerifyCodeType verifyCodeType)
        {
            string Subject;
            string Content;
            switch (verifyCodeType)
            {
                case VerifyCodeType.EmailLogin:
                    Subject = "邮箱验证登录";
                    Content = "您正在通过邮箱验证码登录，";
                    break;
                case VerifyCodeType.EmailBind:
                    Subject = "绑定邮箱";
                    Content = "您正在为账户绑定邮箱，";
                    break;
                case VerifyCodeType.EmailRetrievePassword:
                    Subject = "邮箱验证找回密码";
                    Content = "您正在通过邮箱验证找回密码，";
                    break;
                case VerifyCodeType.EmailChangePassword:
                    Subject = "邮箱验证修改密码";
                    Content = "您正在通过邮箱验证修改密码，";
                    break;
                default:
                    throw new NotSupportedException();
            }
            Content += $"验证码为：{code}，一分钟内有效";
            if (string.IsNullOrWhiteSpace(Content))
                return Task.FromResult(false);
            if (string.IsNullOrWhiteSpace(Content))
                return Task.FromResult(false);
            _smtpClientService.SendEmail(Email,Content,Subject);
            return Task.FromResult(true);
        }

        public Task<bool> SendMessage(string Mobile, string code, VerifyCodeType verifyCodeType)
        {
            return Task.FromResult(true);
        }
    }
}
