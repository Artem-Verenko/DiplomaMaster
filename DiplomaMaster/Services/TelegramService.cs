using Diploma_DataAccess.DTOs;
using DiplomaMaster.Services.Models;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace DiplomaMaster.Services
{
    public class TelegramService : ITelegramService
    {
        private readonly TelegramBotClient _botClient;
        private readonly string _chatId;

        public TelegramService(IOptions<TelegramSettings> settings)
        {
            _botClient = new TelegramBotClient(settings.Value.BotToken);
            _chatId = settings.Value.CHAT_ID;
        }

        public async Task<TelegramResult> SendMessageAsync(string message)
        {
            try
            {
                var result = await _botClient.SendTextMessageAsync(_chatId, message);
                return new TelegramResult
                {
                    IsSuccess = true,
                    Message = "Success: Message was sent successfully."
                };
            }
            catch (ApiRequestException apiEx)
            {
                return new TelegramResult
                {
                    IsSuccess = false,
                    Message = $"Error: Failed to send message - {apiEx.Message}"
                };
            }
        }

        public async Task<TelegramResult> SendPhotoAsync(Stream photoStream, string caption = null)
        {
            try
            {
                var inputPhoto = new InputFileStream(photoStream);
                var result = await _botClient.SendPhotoAsync(_chatId, inputPhoto, null, caption);
                return new TelegramResult
                {
                    IsSuccess = true,
                    Message = "Success: Photo was sent successfully."
                };
            }
            catch (ApiRequestException apiEx)
            {
                return new TelegramResult
                {
                    IsSuccess = false,
                    Message = $"Error: Failed to send photo - {apiEx.Message}"
                };
            }
        }
    }
}
