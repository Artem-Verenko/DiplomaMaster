using Diploma_DataAccess.Data;
using Diploma_Model.Models;
using Diploma_Model.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DiplomaMaster.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.TweetMessage = TempData["TweetMessage"]?.ToString();
            HomeVM homeVM = new HomeVM()
            {
                Posts = _db.Post.Where(p => p.Visible).Include(u => u.Category),
                Categories = _db.Category.OrderBy(c => c.DisplayOrder)
            };
            return View(homeVM);
        }

        [HttpGet]
        public IActionResult Details(int id)
        { 

            DetailsVM DetailsVM = new DetailsVM()
            {
                Post = _db.Post.Include(u => u.Category).Where(u => u.Id == id).FirstOrDefault()
            };
            
            return View(DetailsVM);
        }

        [HttpGet]
        public IActionResult TwitterAnnounce(int id)
        {
            return RedirectToAction("Index", "Twitter", new { id = id });
        }

        [HttpGet]
        public IActionResult TelegramAnnounce(int id)
        {
            return RedirectToAction("Index", "Telegram", new { id = id });
        }
        
        [HttpGet]
        public IActionResult GmailAnnounce(int id)
        {
            return RedirectToAction("Index", "Email", new { id = id });
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
