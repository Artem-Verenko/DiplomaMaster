namespace Diploma_Model.Models.ViewModels
{
    public class TelegramAnnounceVM
    {
        public TelegramAnnounceVM ()
        {
        }

        public TelegramAnnounceVM(Post post)
        {
            Post = post ?? new Post();
            DefaultPrompt = $"Prepare a full post for publication on Telegram using the following details:\r\n\r\nPlatform: DiplomaMaster\r\nPost Title: {Post.Name}\r\nDescription: {Post.ShortDesc}\r\nFull Text: {Post.PostText}\r\n\r\nPlease ensure the post is formatted for clear readability and includes all necessary details for a comprehensive update to the audience. Include relevant hashtags and format it appropriately for the Telegram platform.";
        }

        public Post Post { get; set; }

        public string DefaultPrompt { get; set; }

        public string? Prompt { get; set; }
    }
}