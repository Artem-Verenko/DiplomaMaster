using Diploma_DataAccess.Data;
using Diploma_DataAccess.DTOs;
using Diploma_Model.Models.ViewModels;
using DiplomaMaster.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiplomaMaster.Controllers
{
    public class TwitterController : Controller
    {
        private readonly ITwitterService _twitterService;
        private readonly IOpenAIService _openAIService;
        private readonly IEmailService _emailService;
        public readonly ApplicationDbContext _db;

        public TwitterController(ITwitterService twitterService, ApplicationDbContext applicationDbContext, IOpenAIService openAIService, IEmailService emailService)
        {
            _emailService = emailService;
            _openAIService = openAIService;
            _twitterService = twitterService;
            _db = applicationDbContext;
        }

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

            TwitterAnnounceVM twitterAnnounceVM = new TwitterAnnounceVM(postFromDb);
            return View("Index", twitterAnnounceVM);
        }

        [HttpPost]
        public async Task<IActionResult> TwitterPostGenerate(int id, [FromForm] TwitterAnnounceVM model)
        {
            var postFromDb = await _db.Post.Include(u => u.Category).FirstOrDefaultAsync(u => u.Id == id);

            TwitterAnnounceVM twitterAnnounce = new TwitterAnnounceVM(postFromDb);

            string promptFromForm = model.Prompt;

            if (!string.IsNullOrEmpty(promptFromForm) && promptFromForm != twitterAnnounce.DefaultPrompt)
            {
                twitterAnnounce.Prompt = promptFromForm;
            }

            var openAIResult = await _openAIService.GenerateTextAsync(promptFromForm, 80);
            var textResult = openAIResult.IsSuccess ? openAIResult.Content : "An error occurred during text generation.";
            TempData["Result"] = textResult;
            ViewBag.Prompt = model.Prompt;
            ViewBag.Result = textResult;

            return View("Index", twitterAnnounce);
        }

        [HttpPost]
        public async Task<IActionResult> TwitterPostAnnounce(int id, string EditedResult)
        {
            var textResult = !string.IsNullOrWhiteSpace(EditedResult) ? EditedResult : TempData["Result"]?.ToString();
            if (string.IsNullOrWhiteSpace(textResult))
            {
                TempData["TweetMessage"] = "There is no message to announce.";
                return RedirectToAction(nameof(Index), new { id = id });
            }

            var tweetResult = await _twitterService.PostTweetAsync(textResult);

            if (tweetResult.IsSuccess)
            {
                TempData["TweetMessage"] = "Tweet was sent successfully!";
            }
            else
            {
                TempData["TweetMessage"] = $"Error sending tweet: {tweetResult.Message}";
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
