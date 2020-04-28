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

        public MessageService(ISmtpClientService smtpClientService, IOptions<EmailNoticeOptions> EmailNoticeOptions)
        {
            _smtpClientService = smtpClientService;
            emailNoticeOptions = EmailNoticeOptions;
        }

    

        

        public Task<bool> SendEmailToManagerForAnswerAsync(string Content)
        {
            return SendEmailAsync(RandomAnEmail()?.Email, Content, EmailType.NoticeManager);
        }

        public Task<bool> SendEmailToTopicCreatorAsync(string Email, string Content)
        {
            return SendEmailAsync(Email, Content, EmailType.NoticeTopicCreator);
        }

       

        public Task<bool> SendVerifyCodeForBindEmailAsync(string Email, string Content)
        {

            return SendEmailAsync(Email, Content, EmailType.EmailBind);
        }

        public Task<bool> SendVerifyCodeForChangePasswordAsync(string Email, string Content)
        {

            return SendEmailAsync(Email, Content, EmailType.EmailChangePassword);
        }

        public Task<bool> SendVerifyCodeForLoginWithEmailAsync(string Email, string Content)
        {
            return SendEmailAsync(Email, Content, EmailType.EmailLogin);
        }

        public Task<bool> SendVerifyCodeForRetrievePasswordAsync(string Email, string Content)
        {

            return SendEmailAsync(Email, Content, EmailType.EmailRetrievePassword);
        }
        async Task<bool> SendEmailAsync(string Email, string content, EmailType verifyCodeType)
        {

            if (string.IsNullOrWhiteSpace(Email))
                return false;
            if (string.IsNullOrWhiteSpace(content))
                return false;
            string Subject;
            string Content;
            switch (verifyCodeType)
            {
                case EmailType.EmailLogin:
                    Subject = "邮箱验证登录";
                    Content = $"您正在通过邮箱验证码登录，验证码为：{content}，一分钟内有效";
                    break;

                case EmailType.EmailBind:
                    Subject = "绑定邮箱";
                    Content = $"您正在为账户绑定邮箱，验证码为：{content}，一分钟内有效";
                    break;

                case EmailType.EmailRetrievePassword:
                    Subject = "邮箱验证找回密码";
                    Content = $"您正在通过邮箱验证找回密码，验证码为：{content}，一分钟内有效";
                    break;

                case EmailType.EmailChangePassword:
                    Subject = "邮箱验证修改密码";
                    Content = $"您正在通过邮箱验证修改密码，验证码为：{content}，一分钟内有效";
                    break;

                case EmailType.NoticeManager:
                case EmailType.NoticeTopicCreator:
                    Subject = "Blazor-Blazui社区消息";
                    Content = content;
                    break;

                default:
                    throw new NotSupportedException();
            }
            if (string.IsNullOrWhiteSpace(Content))
                return false;
            if (string.IsNullOrWhiteSpace(Subject))
                return false;
            return await _smtpClientService.SendAsync(Email, Content, Subject);

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