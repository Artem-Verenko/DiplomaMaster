using Diploma_DataAccess.DTOs;
using DiplomaMaster.Services.Models;
using Newtonsoft.Json;
using System.Text;

namespace DiplomaMaster.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _openAIClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OpenAIService> _logger;

        public OpenAIService(IConfiguration configuration, HttpClient openAIClient, ILogger<OpenAIService> logger)
        {
            _configuration = configuration;
            _openAIClient = openAIClient;
            InitializeClient();
            _logger = logger;
        }

        private void InitializeClient()
        {
            var openAPIKey = _configuration["OpenAI:Key"];
            _openAIClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAPIKey}");
        }

        public async Task<TextGenerationResult> GenerateTextAsync(string prompt, int _max_tokens = 2000)
        {
            OpenAIResponseGPT4 response = null;
            var payload = new
            {
                model = "gpt-3.5-turbo-16k",
                messages = new object[]
                 {
                    new { role = "system", content = $"Hi"},
                    new {role = "user", content = prompt}
                 },
                temperature = 0.3,
                max_tokens = _max_tokens
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);
            HttpContent httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            //Send the request
            var responseMessage = await _openAIClient.PostAsync("https://api.openai.com/v1/chat/completions", httpContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseMessageJson = await responseMessage.Content.ReadAsStringAsync();

                //Return a response
                response = JsonConvert.DeserializeObject<OpenAIResponseGPT4>(responseMessageJson);
            }

            return new TextGenerationResult
            {
                IsSuccess = responseMessage.IsSuccessStatusCode,
                Content = response.Choices[0].Message.Content
            };
        }

        public async Task<ImageGenerationResult> GenerateImageAsync(string prompt)
        {
            OpenAIResponseDAALE2 response = null;
            var payload = new
            {
                model = "dall-e-3",
                size = "1024x1024",
                prompt = prompt,
                quality = "standard",
                n = 1,       
            };
            string jsonPayload = JsonConvert.SerializeObject(payload);
            HttpContent httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            //Send the request
            var responseMessage = await _openAIClient.PostAsync("https://api.openai.com/v1/images/generations", httpContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseMessageJson = await responseMessage.Content.ReadAsStringAsync();

                //Return a response
                response = JsonConvert.DeserializeObject<OpenAIResponseDAALE2>(responseMessageJson);
            }

            return new ImageGenerationResult
            {
                IsSuccess = responseMessage.IsSuccessStatusCode,
                ImageUrl = response.Data[0].Url,
                Prompt = prompt
            };        
        }
    }
}
