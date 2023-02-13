namespace BlogApp.Models.Response
{
    public class Message : IResponseData
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
