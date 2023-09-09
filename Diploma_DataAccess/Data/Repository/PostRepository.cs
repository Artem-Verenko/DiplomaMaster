using Diploma_Utility;
using Diploma_Model.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Diploma_DataAccess.Data.Repository.IRepository;

namespace Diploma_DataAccess.Data.Repository
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

