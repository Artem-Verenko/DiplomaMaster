using Diploma_Model.Models;
using Diploma_DataAccess.Data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Diploma_DataAccess.Data.Repository.IRepository
{
    public interface IPostRepository : IRepository<Post>
    {
        void Update(Post obj);

        IEnumerable<SelectListItem> GetAllDropdownList(string obj);
    }
}
