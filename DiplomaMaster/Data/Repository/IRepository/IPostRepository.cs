using DiplomaMaster.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiplomaMaster.Data.Repository.IRepository
{
    public interface IPostRepository : IRepository<Post>
    {
        void Update(Post obj);

        IEnumerable<SelectListItem> GetAllDropdownList(string obj);
    }
}
