using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_Model.Models.ViewModels
{
    public class GmailAnnounceVM
    {
        public GmailAnnounceVM()
        {
        }

        public GmailAnnounceVM(Post post)
        {
            Post = post ?? new Post();
       
            DefaultPrompt = $"Create a clean, responsive HTML email for Gmail, dedicated to announcing a post on the DiplomaMaster platform. The email should strictly contain the following elements and no other text:\r\n\r\nEmail Subject: {Post.Name}\r\nBrief Description: {Post.ShortDesc}\r\nFull Text: {Post.PostText}\r\nEnsure the email is formatted for clear readability and uses inline CSS for the layout of the header and body. The email should only display the information provided above, ready for publication.";
        }

        public Post Post { get; set; }

        public string DefaultPrompt { get; set; }

        public string? Prompt { get; set; }
    }
}
