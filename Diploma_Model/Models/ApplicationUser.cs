using Microsoft.AspNetCore.Identity;

namespace Diploma_Model.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public ApplicationUser()
        {
        }
        public ApplicationUser(string fullName)
        {
            FullName = fullName;
        }
    }
}
