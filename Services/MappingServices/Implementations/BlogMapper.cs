using BlogApp.Models;
using BlogApp.Models.Dtos;

namespace BlogApp.Services.MappingServices.Implementations
{
    public class BlogMapper : IBlogMapper
    {
        public Blog Map(BlogDto dto) => new Blog
        {
            Id = dto.Id,
            Title = dto.Title,
            Content = dto.Content,
            Likes = dto.Likes
        };

        public BlogDto Map(Blog entity) => new BlogDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Content = entity.Content,
            Likes = entity.Likes
        };

        public BlogCommentDto Map(BlogComment entity) => new BlogCommentDto
        {
            Id = entity.Id,
            Text = entity.Text,
            UserId = entity.UserId,
            BlogId = entity.BlogId,
        };

        public BlogComment Map(BlogCommentDto dto, ApplicationUser user) => new BlogComment
        {
            Id = dto.Id,
            Text = dto.Text,
            UserId = user.Id,
            BlogId = dto.BlogId,
            UserName = user.Name
        };

        public BlogOwner Map(BlogUserObject source) => new BlogOwner
        {
            UserId = source.User.Id,
            BlogId = source.Blog.Id,
            OwnerName = source.User.Name
        };
    }
}
