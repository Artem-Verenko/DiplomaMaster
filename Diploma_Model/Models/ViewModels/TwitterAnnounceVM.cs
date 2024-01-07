namespace Diploma_Model.Models.ViewModels
{
    public class TwitterAnnounceVM
    {
        public TwitterAnnounceVM()
        {
        }

        public TwitterAnnounceVM(Post post)
        {
            Post = post ?? new Post();
            DefaultPrompt = $"Create an announcement for publication on Twitter using the following information: Platform : DiplomaMaster, Post Name : {Post?.Name}, Brief description : {Post?.ShortDesc}. Use the twitter format";
        }

        public Post Post { get; set; }

        public string DefaultPrompt { get; set; }

        public string? Prompt { get; set; }

    }
}
