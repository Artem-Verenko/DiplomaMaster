using DiplomaMaster.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaMaster.Controllers
{
    public class OpenAIController: Controller
    {
        private readonly IOpenAIService _openAIService;
        public OpenAIController(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        [HttpGet]
        public IActionResult OpenAI_GPT_4()
        {       
            return View("OpenAI_GPT_4");           
        }

        [HttpPost]
        public async Task<IActionResult> OpenAI_GPT_4(string prompt)
        {
            var result = await _openAIService.GenerateTextAsync(prompt);
            ViewBag.Prompt = prompt;
            ViewBag.Result = result.IsSuccess ? result.Content : "An error occurred";
            return View("OpenAI_GPT_4");
        }

        [HttpGet]
        public IActionResult DALLE2()
        {
                return View("DALLE2");
        }

        [HttpPost]
        public async Task<IActionResult> DALLE2(string prompt)
        {
            var result = await _openAIService.GenerateImageAsync(prompt);
            ViewBag.Prompt = prompt;
            ViewBag.Result = result.IsSuccess ? result.ImageUrl : "An error occurred";
            return View("DALLE2");
        }

    }
}
