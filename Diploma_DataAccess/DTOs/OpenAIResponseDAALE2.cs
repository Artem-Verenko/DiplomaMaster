namespace Diploma_DataAccess.DTOs
{

    public class OpenAIResponseDAALE2
    {
        public long Created { get; set; }
        public List<DAALE2Image> Data { get; set; }
    }

    public class DAALE2Image
    {
        public string Url { get; set; }
    }

}
