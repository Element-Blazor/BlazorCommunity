﻿using Blazui.Community.Enums;
using System.Threading.Tasks;

namespace Blazui.Community.Api.Service
{
    /// <summary>
    ///
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="Mobile"></param>
        /// <param name="code"></param>
        /// <param name="verifyCodeType"></param>
        /// <returns></returns>
        Task<bool> SendMessageAsync(string Mobile, string code, VerifyCodeType verifyCodeType);

        /// <summary>
        /// 发送邮箱验证码
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="code"></param>
        /// <param name="verifyCodeType"></param>
        /// <returns></returns>
        Task<bool> SendEmailAsync(string Email, string code, VerifyCodeType verifyCodeType);

        /// <summary>
        /// 新的提问发邮件通知
        /// </summary>
        /// <param name="TopicRoute"></param>
        /// <returns></returns>
        Task<bool> EmailNoticeForNewAskOrReplyAsync(string TopicRoute);
    }
}