using Diploma_Utility;
using Diploma_DataAccess.Data;
using Diploma_Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Diploma_DataAccess.Data.Repository.IRepository;

namespace DiplomaMaster.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _catRepo;

        public CategoryController(ICategoryRepository cetRepo)
        {
            _catRepo = cetRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> objList = await _catRepo.GetAllAsync();
            objList = objList.OrderBy(c => c.DisplayOrder);
            return View(objList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _catRepo.Add(obj);
                await _catRepo.SaveAsync();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

         [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = await _catRepo.FindAsync(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                await _catRepo.UpdateAsync(obj);
                await _catRepo.SaveAsync();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = await _catRepo.FindAsync(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var obj = await _catRepo.FindAsync(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            _catRepo.Remove(obj);
            await _catRepo.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}
