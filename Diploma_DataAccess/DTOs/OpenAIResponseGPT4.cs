namespace Diploma_DataAccess.DTOs
{
    public class OpenAIResponseGPT4
    {
        public string id { get; set; }
        public string Object { get; set; }
        public int Created { get; set; }
        public string Model { get; set; }
        public List<Choice> Choices { get; set; }   
    }
     
    public class Choice
    {
        public MessageGpt Message { get; set; }
        public string FinishReason { get; set; }
        public int Index { get; set; }
    }   

    public class MessageGpt
    {
        public string Role { get; set; } 
        public string Content { get; set; }
    }
}
