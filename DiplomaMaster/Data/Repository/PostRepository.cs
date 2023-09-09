using Diploma_Utility;
using DiplomaMaster.Data.Repository.IRepository;
using DiplomaMaster.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiplomaMaster.Data.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly ApplicationDbContext _db;
        public PostRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetAllDropdownList(string obj)
        {
            if (obj == WC.CategoryName)
            {
                return _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }        
            return null;
        }

        public void Update(Post obj)
        {
            _db.Post.Update(obj);
        }
    }
}

