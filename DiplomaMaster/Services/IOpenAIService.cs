using DiplomaMaster.Services.Models;

namespace DiplomaMaster.Services
{
    public interface IOpenAIService
    {
        Task<TextGenerationResult> GenerateTextAsync(string prompt, int _max_tokens = 500);
        Task<ImageGenerationResult> GenerateImageAsync(string prompt);
    }
}
