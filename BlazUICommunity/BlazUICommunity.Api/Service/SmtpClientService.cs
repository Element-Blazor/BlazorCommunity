using Blazui.Community.Api.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazui.Community.Api.Service
{
    public class SmtpClientService : ISmtpClientService
    {
        private static EmailConfig emailConfig;
        private ILogger<SmtpClientService> _logger;
        public SmtpClientService(IOptions<EmailConfig> options, ILogger<SmtpClientService> logger)
        {
            emailConfig = options.Value;
            _logger = logger;
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
            try
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
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                client.Timeout = 30000;
                client.Connect(emailConfig.StmpHost, emailConfig.StmpPort, true);
                stopwatch.Stop();
                _logger.LogDebug("SendEmail-Connect：" + stopwatch.ElapsedMilliseconds);
            
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(emailConfig.Account, emailConfig.Auth);
                stopwatch.Reset();
                await client.SendAsync(message);
                stopwatch.Stop();
                _logger.LogDebug("SendEmail-SendAsync：" + stopwatch.ElapsedMilliseconds);
                client.Disconnect(true);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message);
                _logger.LogDebug(ex.StackTrace);
            }
        }



    }
}
