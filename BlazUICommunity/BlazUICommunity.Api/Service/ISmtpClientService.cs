using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Api.Service
{
    public interface ISmtpClientService
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="ToEmail"></param>
        /// <param name="Content"></param>
        /// <param name="Subject"></param>
        /// <returns></returns>
        Task SendEmail(string ToEmail, string Content, string Subject);
    }
}
