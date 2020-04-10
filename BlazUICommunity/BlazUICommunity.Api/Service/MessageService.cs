using Blazui.Community.Api.Options;
using Blazui.Community.Enums;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Api.Service
{
    public class MessageService : IMessageService
    {
        private readonly ISmtpClientService _smtpClientService;
        private readonly IOptions<EmailNoticeOptions> emailNoticeOptions;
        private readonly IOptions<BaseDomainOptions> domainOption;

        public MessageService(ISmtpClientService smtpClientService, IOptions<EmailNoticeOptions> EmailNoticeOptions,IOptions<BaseDomainOptions> DomainOption)
        {
            _smtpClientService = smtpClientService;
            _smtpClientService = smtpClientService;
            emailNoticeOptions = EmailNoticeOptions;
            domainOption = DomainOption;
        }

        public async Task<bool> EmailNoticeForNewAskOrReplyAsync(string TopicRoute)
        {
            var noticeEmail = RandomAnEmail();
            if(noticeEmail !=null)
            {
                await _smtpClientService.SendAsync(noticeEmail.Email, $"刚刚有人在社区发表了提问，请尽快回复，链接地址：{domainOption.Value.BaseDomain}{TopicRoute}", "Blazui社区通知您");
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> SendEmailAsync(string Email, string code, VerifyCodeType verifyCodeType)
        {

            if (string.IsNullOrWhiteSpace(Email))
                return await Task.FromResult(false);
            if (string.IsNullOrWhiteSpace(code))
                return await Task.FromResult(false);
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
                await Task.FromResult(false);
            if (string.IsNullOrWhiteSpace(Subject))
                await Task.FromResult(false);
            await  _smtpClientService.SendAsync(Email, Content, Subject);
           return await Task.FromResult(true);
        }

        public Task<bool> SendMessageAsync(string Mobile, string code, VerifyCodeType verifyCodeType)
        {
            return Task.FromResult(true);
        }


        private EmailNoticeModel RandomAnEmail()
        {
            if (emailNoticeOptions.Value.EmailNotices is null)
                return null;
            var Notices = emailNoticeOptions.Value.EmailNotices
                .Select(s => new KeyValuePair<EmailNoticeModel, int>(s, s.Weight));
            List<EmailNoticeModel> emailNoticeModels = new List<EmailNoticeModel>();//总数等于权重的和
            foreach (var sw in Notices)
            {
                for (int i = 0; i < sw.Value; i++)
                {
                    emailNoticeModels.Add(sw.Key);
                }
            }
            int total = Notices.Sum(s => s.Value);
            int index = new Random().Next(0, total);//左边闭区间  右边开区间
            return emailNoticeModels[index];
        }
    }
}