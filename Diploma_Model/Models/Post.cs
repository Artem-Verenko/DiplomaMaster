using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Diploma_Model.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [AllowNull]
        public string? ShortDesc { get; set; }

        [AllowNull]
        public string? PostText { get; set; }

        public string Image { get; set; }
        public DateTime PublishedDate { get; set; } = DateTime.Now;
        public string? Author { get; set; }
        public bool Visible { get; set; }

        [Display(Name = "Category Type")]
        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
