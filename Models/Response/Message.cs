namespace BlogApp.Models.Response
{
    public class Message : IResponseData
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
