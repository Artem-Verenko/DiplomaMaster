using Diploma_DataAccess.Data.Repository.IRepository;
using Diploma_Model.Models;
using Diploma_Model.Models.ViewModels;
using Diploma_Utility;
using DiplomaMaster.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaMaster.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IOpenAIService _openAIService;

        public PostController(IPostRepository prodRepo, IWebHostEnvironment webHostEnvironment, IOpenAIService openAIService)
        {
            _postRepo = prodRepo;
            _webHostEnvironment = webHostEnvironment;
            _openAIService = openAIService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Post> objList = await _postRepo.GetAllAsync(includeProperties: "Category");
            return View(objList);
        }


        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
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
                postVM.Post = await _postRepo.FindAsync(id.GetValueOrDefault());
                if (postVM.Post == null)
                {
                    return NotFound();
                }
                return View(postVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(PostVM postVM, string imageSource, string imageCaption)
        {
            if (/*ModelState.IsValid*/ true)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                string extension;
                string upload = webRootPath + WC.ImagePath;
                string fileName = Guid.NewGuid().ToString();             

                if (postVM.Post.Id == 0)
                {
                    //Creating
                    if (imageSource == "local")
                    {
                        extension = Path.GetExtension(files[0].FileName);

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                    }
                    else if (imageSource == "generate")
                    {
                        extension = ".png";

                        var result = await _openAIService.GenerateImageAsync(imageCaption);

                        string generatedImageURL = result.IsSuccess ? result.ImageUrl : "An error occurred";

                        using (HttpClient client = new HttpClient())
                        {
                            var response = await client.GetAsync(generatedImageURL);
                            response.EnsureSuccessStatusCode();

                            byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                            string savePath = Path.Combine(upload, fileName + extension);

                            await System.IO.File.WriteAllBytesAsync(savePath, imageBytes);

                        }
                        postVM.Post.Image = fileName + extension;
                    }
                    else
                    {
                        TempData[WC.Error] = "Invalid image source selected.";
                        postVM.CategorySelectList = _postRepo.GetAllDropdownList(WC.CategoryName);
                        return View(postVM);
                    }

                    postVM.Post.Image = fileName + extension;

                    postVM.Post.PublishedDate = DateTime.Now;
                
                   
                    postVM.Post.Author = User.Identity.Name;

                    _postRepo.Add(postVM.Post);
                }
                else
                {
                    //updating
                    var objFromDb = await _postRepo.FirstOrDefaultAsync(u => u.Id == postVM.Post.Id, isTracking: false);
                    var oldFile = Path.Combine(upload, objFromDb.Image);

                    if (imageSource == "local" && files.Count > 0)
                    {
                        extension = Path.GetExtension(files[0].FileName);

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
                    else if (imageSource == "generate")
                    {
                        extension = ".png";

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        var result = await _openAIService.GenerateImageAsync(imageCaption);
       
                        string generatedImageURL = result.IsSuccess ? result.ImageUrl : "An error occurred";

                        using (HttpClient client = new HttpClient())
                        {
                            var response = await client.GetAsync(generatedImageURL);
                            response.EnsureSuccessStatusCode();

                            byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                            string savePath = Path.Combine(upload, fileName + extension);

                            await System.IO.File.WriteAllBytesAsync(savePath, imageBytes);

                        }
                        postVM.Post.Image = fileName + extension;
                    }
                    else
                    {
                        postVM.Post.Image = objFromDb.Image;
                    }

                    postVM.Post.PublishedDate = DateTime.Now;

                    postVM.Post.Author = User.Identity.Name;

                    _postRepo.Update(postVM.Post);
                }

                await _postRepo.SaveAsync();  // Save changes
                return RedirectToAction("Index");  // Redirect to index after successful creation/update
            }

            postVM.CategorySelectList = _postRepo.GetAllDropdownList(WC.CategoryName);
            return View(postVM);
        }

       

       
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Post product = await _postRepo.FirstOrDefaultAsync(u => u.Id == id, includeProperties: "Category");
            //product.Category = _db.Category.Find(product.CategoryId);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var obj = await _postRepo.FindAsync(id.GetValueOrDefault());
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
            await _postRepo.SaveAsync();
            TempData[WC.Success] = "Action completed successfully";
            return RedirectToAction("Index");
        }      
    }
}