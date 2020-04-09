using Blazui.Community.Api.Configuration;
using Blazui.Community.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            this._smtpClientService = smtpClientService;
            emailNoticeOptions = EmailNoticeOptions;
        }

        public async Task<bool> EmailNoticeForNewAskOrReplyAsync(string TopicUrl)
        {
            var noticeEmail = RandomAnEmail();
            if(noticeEmail !=null)
            {
                await _smtpClientService.SendAsync(noticeEmail.Email, $"问题地址：{TopicUrl}", "Blazui社区有人发布了新的提问，请尽快去回复！");
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> SendEmailAsync(string Email, string code, VerifyCodeType verifyCodeType)
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
                await Task.FromResult(false);
            if (string.IsNullOrWhiteSpace(Content))
                await Task.FromResult(false);
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            await  _smtpClientService.SendAsync(Email, Content, Subject);
            //stopwatch.Stop();
            //Console.WriteLine("_smtpClientService-SendEmail：" + stopwatch.ElapsedMilliseconds);
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