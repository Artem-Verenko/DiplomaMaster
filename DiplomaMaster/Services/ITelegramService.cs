using DiplomaMaster.Services.Models;

namespace DiplomaMaster.Services
{
    public interface ITelegramService
    {
        Task<TelegramResult> SendMessageAsync(string message);
        Task<TelegramResult> SendPhotoAsync(Stream photoStream, string caption = null);
    }
}
