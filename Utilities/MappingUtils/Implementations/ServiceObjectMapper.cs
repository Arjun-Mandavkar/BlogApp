using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.ServiceObjects;
using BloggingApplication.Models.Dtos;

namespace BlogApp.Utilities.MappingUtils.Implementations
{
    public class ServiceObjectMapper : IServiceObjectMapper
    {
        public BlogServiceObject Map(Blog source) => new BlogServiceObject
        {
            Id = source.Id,
            Title = source.Title,
            Content = source.Content,
            Likes = source.Likes
        };

        public BlogDto Map(BlogServiceObject source) => new BlogDto
        {
            Id = source.Id,
            Title = source.Title,
            Content = source.Content,
            Likes = source.Likes
        };

        public BlogCommentServiceObject Map(BlogComment source) => new BlogCommentServiceObject
        {
            Id = source.Id,
            BlogId = source.BlogId,
            UserId = source.UserId,
            Text = source.Text,
            TimeStamp = source.TimeStamp
        };

        public BlogCommentDto Map(BlogCommentServiceObject source) => new BlogCommentDto
        {
            Id = source.Id,
            BlogId = source.BlogId,
            UserId = source.UserId,
            Text = source.Text
        };

        public UserServiceObject Map(ApplicationUser source) => new UserServiceObject
        {
            Id = source.Id,
            Name = source.Name,
            Email = source.Email,
            Role = source.Role,
            PasswordHash = source.PasswordHash,
        };

        public UserInfoDto Map(UserServiceObject source) => new UserInfoDto
        {
            Id = source.Id,
            Name = source.Name,
            Email = source.Email,
            Role = source.Role.ToString()
        };

        public AuthUserInfoDto Map(UserServiceObject user, string token) => new AuthUserInfoDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString(),
            Token = token
        };
    }
}
