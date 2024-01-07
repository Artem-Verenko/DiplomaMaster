namespace DiplomaMaster.Services.Models
{
    public class ImageGenerationResult
    {
        public bool IsSuccess { get; set; }
        public string ImageUrl { get; set; }
        public string Prompt { get; set; }
    }
}
