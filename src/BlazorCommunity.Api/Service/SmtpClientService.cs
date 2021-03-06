﻿using BlazorCommunity.Api.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCommunity.Api.Service
{
    public class SmtpClientService : ISmtpClientService
    {
        private static EmailStmpConfig emailConfig;
        private ILogger<SmtpClientService> _logger;

        /// <summary>
        ///
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public SmtpClientService(IOptionsMonitor<EmailStmpOption> options, ILogger<SmtpClientService> logger)
        {
            emailConfig = options.CurrentValue.EmailSetting;
            _logger = logger;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ToEmail"></param>
        /// <param name="Content"></param>
        /// <param name="Subject"></param>
        /// <returns></returns>
        public async Task<bool> SendAsync(string ToEmail, string Content, string Subject)
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
                await client.ConnectAsync(emailConfig.StmpHost, emailConfig.StmpPort, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(emailConfig.Account, emailConfig.Auth);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex.Message);
                _logger.LogDebug(ex.StackTrace);
                return false;
            }
        }
    }
}