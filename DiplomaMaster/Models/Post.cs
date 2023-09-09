using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DiplomaMaster.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? ShortDesc { get; set; }

        public string? PostText { get; set; }

        [AllowNull]
        public string? Image { get; set; }

        [AllowNull]
        [Display(Name = "Category Type")]
        public int? CategoryId { get; set; }

        [AllowNull]
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
