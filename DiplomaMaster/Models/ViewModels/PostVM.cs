using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics.CodeAnalysis;

namespace DiplomaMaster.Models.ViewModels
{
    public class PostVM
    {
        [AllowNull]
        public Post? Post { get; set; }
        
        [AllowNull]
        public IEnumerable<SelectListItem>? CategorySelectList { get; set; }
        
        [AllowNull]
        public IEnumerable<SelectListItem>? ApplicationTypeSelectList { get; set; }
    }
}
