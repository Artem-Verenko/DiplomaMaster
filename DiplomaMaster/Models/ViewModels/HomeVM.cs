namespace DiplomaMaster.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Post> Posts { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
