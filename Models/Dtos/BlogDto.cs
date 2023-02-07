using BlogApp.Models.Response;

namespace BlogApp.Models.Dtos
{
    public class BlogDto : IResponseData
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Likes { get; set; }
    }
}
