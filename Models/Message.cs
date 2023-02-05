namespace BlogApp.Models
{
    public class Message : IResponseData
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
