using Blazui.Community.Api.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazui.Community.Api.Service
{
    public class SmtpClientService : ISmtpClientService
    {
        private static EmailConfig emailConfig;

        public SmtpClientService(IOptions<EmailConfig> options)
        {
            emailConfig = options.Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ToEmail"></param>
        /// <param name="Content"></param>
        /// <param name="Subject"></param>
        /// <returns></returns>
        public async Task SendEmail(string ToEmail, string Content, string Subject)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(Encoding.UTF8.GetString(Encoding.Default.GetBytes(emailConfig.FromName)), emailConfig.Account));
            message.To.Add(new MailboxAddress(ToEmail, ToEmail));
            message.Subject = Subject;
            var multipart = new Multipart("mixed")
            {
                new Multipart("alternative") {
                        new TextPart("plain")
                        {Text = Content } }
            };
            message.Body = multipart;
            using var client = new MailKit.Net.Smtp.SmtpClient();
            client.Connect(emailConfig.StmpHost, emailConfig.StmpPort, false);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(emailConfig.Account, emailConfig.Auth);
            await client.SendAsync(message);
            client.Disconnect(true);
        }
    }
}
