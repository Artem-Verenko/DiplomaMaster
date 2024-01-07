using Diploma_DataAccess.Data;
using Diploma_DataAccess.DTOs;
using Diploma_Model.Models.ViewModels;
using DiplomaMaster.Services;
using DiplomaMaster.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiplomaMaster.Controllers
{
    public class TelegramController : Controller
    {
        private readonly ITelegramService _telegramService;
        private readonly IOpenAIService _openAIService;
        public readonly ApplicationDbContext _db;

        public TelegramController(ITelegramService telegramService, IOpenAIService openAIService, ApplicationDbContext applicationDbContext)
        {
            _telegramService = telegramService;
            _openAIService = openAIService;
            _db = applicationDbContext;
        }

        // GET: Telegram
        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            var postFromDb = await _db.Post
                .Include(u => u.Category)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (postFromDb == null)
            {
                return NotFound();
            }

            TelegramAnnounceVM twitterAnnounceVM = new TelegramAnnounceVM(postFromDb);
            return View("Index", twitterAnnounceVM);
        }

        [HttpPost]
        public async Task<IActionResult> TelegramPostGenerate(int id, [FromForm] TelegramAnnounceVM model)
        {
            var postFromDb = await _db.Post.Include(u => u.Category).FirstOrDefaultAsync(u => u.Id == id);

            TelegramAnnounceVM telegramAnnounce = new TelegramAnnounceVM(postFromDb);

            string promptFromForm = model.Prompt;

            if (!string.IsNullOrEmpty(promptFromForm) && promptFromForm != telegramAnnounce.DefaultPrompt)
            {
                telegramAnnounce.Prompt = promptFromForm;
            }

            var openAIResult = await _openAIService.GenerateTextAsync(promptFromForm);
            var textResult = openAIResult.IsSuccess ? openAIResult.Content : "An error occurred during text generation.";
            TempData["Result"] = textResult;
            ViewBag.Prompt = model.Prompt;
            ViewBag.Result = textResult;

            return View("Index", telegramAnnounce);
        }

        [HttpPost]
        public async Task<IActionResult> TelegramPostPublish(int id)
        {
            var textResult = TempData["Result"]?.ToString();
            if (string.IsNullOrWhiteSpace(textResult))
            {
                TempData["TelegramMessage"] = "There is no message to announce.";
                return RedirectToAction(nameof(Index), new { id = id });
            }


           var telegramResult =  await _telegramService.SendMessageAsync(textResult);

            if (telegramResult.IsSuccess)
            {
                TempData["TelegramMessage"] = "Tweet was sent successfully!";
            }
            else
            {
                TempData["TelegramMessage"] = $"Error sending tweet: {telegramResult.Message}";
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
