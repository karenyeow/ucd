using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Common.Helpers.Email
{
    public class OneGovEmailSender : IOneGovEmailSender
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _emailFrom;
        private readonly string _emailTo;
        private readonly string _environment;
        private readonly string _appName;
        private readonly string _securityCodeEmailFrom;

        public OneGovEmailSender(IConfiguration configuration)
        {
            var emailSection = configuration.GetSection("Email");
            var appSection = configuration.GetSection("AppInfo");

            _smtpHost = emailSection["SMTPHost"];
            _smtpPort = Convert.ToInt32(emailSection["SMTPPort"]);
            _emailFrom = emailSection["ErrorEmailFrom"];
            _emailTo = emailSection["ErrorEmailTo"];
            _environment = appSection["Environment"];
            _appName = appSection["ApplicationName"];
            _securityCodeEmailFrom = emailSection["SecurityCodeEmailFrom"];
        }

        public async Task<bool> SendErrorEmailAsync(IExceptionHandlerFeature exception)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_emailFrom, _emailFrom));
                emailMessage.To.Add(new MailboxAddress(_emailTo, _emailTo));
                emailMessage.Subject = $"{_environment} - {_appName} - {exception.Error.Message}";
                emailMessage.Body = new TextPart("plain") { Text = exception.Error.ToString() };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpHost, _smtpPort);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true, CancellationToken.None);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SendErrorEmailSync(IExceptionHandlerFeature exception)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_emailFrom, _emailFrom));
                emailMessage.To.Add(new MailboxAddress(_emailTo, _emailTo));
                emailMessage.Subject = $"{_environment} - {_appName} - {exception.Error.Message}";
                emailMessage.Body = new TextPart("plain") { Text = exception.Error.StackTrace };

                using (var client = new SmtpClient())
                {
                    client.Connect(_smtpHost, _smtpPort);
                    client.Send(emailMessage);
                    client.Disconnect(true, CancellationToken.None);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SendEmailAsync(string message, string emailTo, string subject)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_securityCodeEmailFrom, _securityCodeEmailFrom));
                emailMessage.To.Add(new MailboxAddress(emailTo, emailTo));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart("html") { Text = message };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpHost, _smtpPort);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true, CancellationToken.None);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SendErrorEmailSync(Exception exception)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_emailFrom, _emailFrom));
                emailMessage.To.Add(new MailboxAddress(_emailTo, _emailTo));
                emailMessage.Subject = $"{_environment} - {_appName} - {exception.Message}";
                emailMessage.Body = new TextPart("plain") { Text = exception.StackTrace };

                using (var client = new SmtpClient())
                {
                    client.Connect(_smtpHost, _smtpPort);
                    client.Send(emailMessage);
                    client.Disconnect(true, CancellationToken.None);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SendErrorEmailAsync(Exception exception)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_emailFrom, _emailFrom));
                emailMessage.To.Add(new MailboxAddress(_emailTo, _emailTo));
                emailMessage.Subject = $"{_environment} - {_appName} - {exception.Message}";
                emailMessage.Body = new TextPart("plain") { Text = exception.ToString() };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpHost, _smtpPort);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true, CancellationToken.None);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
