using Diploma_DataAccess.DTOs;
using DiplomaMaster.Services.Models;

namespace DiplomaMaster.Services
{
    public interface IEmailService
    {
        Task<GmailResult> SendEmailAsync(EmailMessage emailMessage);
    }
}
