﻿using MailKit.Net.Smtp;
using MedScanAI.Domain.Helpers;
using MedScanAI.Service.Abstracts;
using MedScanAI.Shared.Base;
using MimeKit;

namespace MedScanAI.Service.Implementation
{
    internal class SendEmailService : ISendEmailService
    {
        private readonly EmailSettings _emailSettings;

        public SendEmailService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public async Task<ReturnBase<bool>> SendEmailAsync(string email, string message, string subject, string contentType = "text/plain")
        {
            try
            {
                MimeMessage emailMessage = new();

                MailboxAddress email_From = new
                    (_emailSettings.SenderHeader, _emailSettings.EmailAddress);

                emailMessage.From.Add(email_From);

                MailboxAddress email_To = new(_emailSettings.SenderHeader, email);
                emailMessage.To.Add(email_To);
                emailMessage.Subject = subject;

                BodyBuilder emailBodyBuilder = new();
                if (contentType == "text/html")
                    emailBodyBuilder.HtmlBody = message;
                else
                    emailBodyBuilder.TextBody = message;

                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                using SmtpClient MailClient = new();
                await MailClient.ConnectAsync(_emailSettings.Host, _emailSettings.Port, _emailSettings.UseSSL);
                await MailClient.AuthenticateAsync(_emailSettings.EmailAddress, _emailSettings.Password);

                await MailClient.SendAsync(emailMessage);

                MailClient.Disconnect(true);
                MailClient.Dispose();

                return ReturnBaseHandler.Success(true, "");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.InnerException.Message);
            }
        }
    }
}
