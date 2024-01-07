using Diploma_DataAccess.Data;
using Diploma_DataAccess.DTOs;
using Diploma_Model.Models.ViewModels;
using DiplomaMaster.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiplomaMaster.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IOpenAIService _openAIService;
        public readonly ApplicationDbContext _db;

        public EmailController(IEmailService emailService, IOpenAIService openAIService, ApplicationDbContext applicationDbContext)
        {
            _emailService = emailService;
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

            GmailAnnounceVM gmailAnnounceVM = new GmailAnnounceVM(postFromDb);
            return View("Index", gmailAnnounceVM);
        }

        [HttpPost]
        public async Task<IActionResult> EmailPostGenerate(int id, [FromForm] GmailAnnounceVM model)
        {
            var postFromDb = await _db.Post.Include(u => u.Category).FirstOrDefaultAsync(u => u.Id == id);

            GmailAnnounceVM telegramAnnounce = new GmailAnnounceVM(postFromDb);

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
        public async Task<IActionResult> EmailPostPublish(int id, string EditedResult)
        {
            var textResult = !string.IsNullOrWhiteSpace(EditedResult) ? EditedResult : TempData["Result"]?.ToString();
            if (string.IsNullOrWhiteSpace(textResult))
            {
                TempData["EmailMessage"] = "There is no message to announce.";
                return RedirectToAction(nameof(Index), new { id = id });
            }


            var gmailResult = await _emailService.SendEmailAsync( new EmailMessage("artem.verenko@gmail.com", "Post", textResult));

            if (gmailResult.IsSuccess)
            {
                TempData["EmailMessage"] = "Tweet was sent successfully!";
            }
            else
            {
                TempData["EmailMessage"] = $"Error sending tweet: {gmailResult.Message}";
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
