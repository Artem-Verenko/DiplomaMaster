using Diploma_DataAccess.DTOs;
using DiplomaMaster.Services.Models;
using System.Net;
using System.Net.Mail;

namespace DiplomaMaster.Services
{
    public class EmailService: IEmailService, IDisposable
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromAddress;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _logger = logger;
            _smtpClient = new SmtpClient(configuration["EmailSettings:Host"])
            {
                Port = int.Parse(configuration["EmailSettings:Port"]),
                Credentials = new NetworkCredential(
                    configuration["EmailSettings:Username"],
                    configuration["EmailSettings:Password"]
                ),
                EnableSsl = bool.Parse(configuration["EmailSettings:EnableSsl"])
            };

            _fromAddress = configuration["EmailSettings:FromAddress"];
        }

        public async Task<GmailResult> SendEmailAsync(EmailMessage emailMessage)
        {
            try
            {
                using (var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromAddress),
                    Subject = emailMessage.Subject,
                    Body = emailMessage.Body,
                    IsBodyHtml = true
                })
                {
                    mailMessage.To.Add(emailMessage.To);
                    await _smtpClient.SendMailAsync(mailMessage);
                }
                return new GmailResult
                {
                    IsSuccess = true,
                    Message = "Success: Message was sent successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending email.");
                return new GmailResult
                {
                    IsSuccess = false,
                    Message = $"Error: Failed to send message"
                };
            }
        }
        public void Dispose()
        {
            _smtpClient?.Dispose();
        }
    }
}
