using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Models.ServiceObjects;

namespace BlogApp.Services.BlogServices
{
    public interface IBlogCommentService
    {
        public Task<ServiceResult> CommentOnBlog(BlogCommentDto comment);
        public Task<ServiceResult> DeleteComment(int commentId);
        public Task<ServiceResult> EditComment(BlogCommentDto comment);
        public Task<IEnumerable<BlogCommentServiceObject>> GetAllCommentsOfBlog(int blogId);
    }
}
