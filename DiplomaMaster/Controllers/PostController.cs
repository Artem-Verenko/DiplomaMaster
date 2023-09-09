using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Diploma_Utility;
using DiplomaMaster.Data.Repository.IRepository;
using DiplomaMaster.Models;
using DiplomaMaster.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace DiplomaMaster.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PostController(IPostRepository prodRepo, IWebHostEnvironment webHostEnvironment)
        {
            _postRepo = prodRepo;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            IEnumerable<Post> objList = _postRepo.GetAll(includeProperties: "Category");
            return View(objList);
        }


        //GET - UPSERT
        public IActionResult Upsert(int? id)
        {
            PostVM postVM = new PostVM()
            {
                Post = new Post(),
                CategorySelectList = _postRepo.GetAllDropdownList(WC.CategoryName),
            };

            if (id == null)
            {
                //this is for create
                return View(postVM);
            }
            else
            {
                postVM.Post = _postRepo.Find(id.GetValueOrDefault());
                if (postVM.Post == null)
                {
                    return NotFound();
                }
                return View(postVM);
            }
        }


        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(PostVM postVM)
        {
            if (/*ModelState.IsValid*/ true)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (postVM.Post.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    postVM.Post.Image = fileName + extension;

                    _postRepo.Add(postVM.Post);
                }
                else
                {
                    //updating
                    var objFromDb = _postRepo.FirstOrDefault(u => u.Id == postVM.Post.Id, isTracking: false);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        postVM.Post.Image = fileName + extension;
                    }
                    else
                    {
                        postVM.Post.Image = objFromDb.Image;
                    }
                    _postRepo.Update(postVM.Post);
                }
                TempData[WC.Success] = "Action completed successfully";

                _postRepo.Save();
                return RedirectToAction("Index");
            }
            postVM.CategorySelectList = _postRepo.GetAllDropdownList(WC.CategoryName);

            return View(postVM);

        }



        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Post product = _postRepo.FirstOrDefault(u => u.Id == id, includeProperties: "Category,ApplicationType");
            //product.Category = _db.Category.Find(product.CategoryId);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        //POST - DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _postRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }


            _postRepo.Remove(obj);
            _postRepo.Save();
            TempData[WC.Success] = "Action completed successfully";
            return RedirectToAction("Index");


        }

    }
}