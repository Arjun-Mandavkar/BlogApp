using BlogApp.Models.Response;

namespace BlogApp.Models.Dtos
{
    public class BlogCommentDto : IResponseData
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
