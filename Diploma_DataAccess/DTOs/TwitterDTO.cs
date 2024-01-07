using Newtonsoft.Json;

namespace Diploma_DataAccess.DTOs
{
    public class TwitterDTO
    {
        [JsonProperty("text")]
        public string Text { get; set; } = string.Empty;
    }
}
