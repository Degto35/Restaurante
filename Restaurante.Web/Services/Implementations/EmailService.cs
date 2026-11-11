using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Restaurante.Web.Core;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailService> _logger;
        private readonly IHostEnvironment _environment;

        public EmailService(IOptions<EmailSettings> settings, ILogger<EmailService> logger, IHostEnvironment environment)
        {
            _settings = settings.Value;
            _logger = logger;
            _environment = environment;
        }

        public async Task<bool> SendAsync(string toEmail, string subject, string htmlBody)
        {
            if (!_settings.IsConfigured)
            {
                if (_environment.IsDevelopment())
                {
                    
                    _logger.LogWarning(
                        "Email is not configured. Message that would be sent to {To} ({Subject}):\n{Body}",
                        toEmail, subject, WebUtility.HtmlDecode(htmlBody));
                }
                else
                {
                    
                    _logger.LogWarning("Email is not configured. A message for {To} was not sent.", toEmail);
                }

                return false;
            }

            try
            {
                using SmtpClient client = new SmtpClient(_settings.Host, _settings.Port)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(_settings.User, _settings.Password)
                };

                using MailMessage message = new MailMessage
                {
                    From = new MailAddress(_settings.User!, _settings.FromName),
                    Subject = subject,
                    Body = htmlBody,
                    IsBodyHtml = true
                };

                message.To.Add(toEmail);

                await client.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not send the email to {To}.", toEmail);
                return false;
            }
        }
    }
}