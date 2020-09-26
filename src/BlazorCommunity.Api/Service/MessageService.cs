using BlazorCommunity.Api.Options;
using BlazorCommunity.Enums;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCommunity.Api.Service
{
    public class MessageService : IMessageService
    {
        private readonly ISmtpClientService _smtpClientService;
        private readonly IOptions<EmailNoticeOptions> emailNoticeOptions;
        private readonly IOptions<BaseDomainOptions> domainOption;

        public MessageService(ISmtpClientService smtpClientService, IOptions<EmailNoticeOptions> EmailNoticeOptions, IOptions<BaseDomainOptions> domainOption)
        {
            _smtpClientService = smtpClientService;
            emailNoticeOptions = EmailNoticeOptions;
            this.domainOption = domainOption;
        }

        public Task<bool> SendEmailToManagerForAnswerAsync(string TopicId)
        {
            var content = $"社区有新的提问，还请尽快回复一下，谢谢，链接地址：{domainOption.Value.BaseDomain}topic/{TopicId}";
            return SendEmailAsync(RandomAnEmail()?.Email, content, "Blazor-Element社区消息");
        }

        public Task<bool> SendEmailToTopicCreatorAsync(string Email, string TopicTitle, string TopicId)
        {
            var content = $"您的帖子—{TopicTitle}，有新的回复，查看链接：{domainOption.Value.BaseDomain}topic/{TopicId}";
            return SendEmailAsync(Email, content, "Blazor-Element社区消息");
        }

        public Task<bool> SendVerifyCodeForBindEmailAsync(string Email, string Code)
        {
            var Subject = "绑定邮箱";
            var Content = $"您正在为账户绑定邮箱，验证码为：{Code}，2分钟内有效";
            return SendEmailAsync(Email, Content, Subject);
        }

        public Task<bool> SendVerifyCodeForChangePasswordAsync(string Email, string Code)
        {
            var Subject = "修改密码";
            var Content = $"您正在通过邮箱验证修改密码，验证码为：{Code}，2分钟内有效";
            return SendEmailAsync(Email, Content, Subject);
        }

        public Task<bool> SendVerifyCodeForLoginWithEmailAsync(string Email, string Code)
        {
            var Subject = "登录验证";
            var Content = $"您正在通过邮箱验证码登录，验证码为：{Code}，2分钟内有效";
            return SendEmailAsync(Email, Content, Subject);
        }

        public Task<bool> SendVerifyCodeForRetrievePasswordAsync(string Email, string Code)
        {
            var Subject = "找回密码";
            var Content = $"您正在通过邮箱验证找回密码，验证码为：{Code}，2分钟内有效";
            return SendEmailAsync(Email, Content, Subject);
        }
        public Task<bool> SendVerifyCodeForUnBindEmailAsync(string Email, string Code)
        {
            var Subject = "邮箱解绑";
            var Content = $"您正在解绑邮箱，验证码为：{Code}，2分钟内有效";
            return SendEmailAsync(Email, Content, Subject);
        }
        async Task<bool> SendEmailAsync(string Email, string Content, string Subject)
        {
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