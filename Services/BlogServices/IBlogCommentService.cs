using BlogApp.Models.Dtos;
using BlogApp.Models;
using BlogApp.Models.Response;

namespace BlogApp.Services.BlogServices
{
    public interface IBlogCommentService
    {
        public Task<ServiceResult> CommentOnBlog(BlogCommentDto comment);
        public Task<ServiceResult> DeleteComment(BlogCommentDto comment);
        public Task<ServiceResult> EditComment(BlogCommentDto comment);
        public Task<IEnumerable<BlogComment>> GetAllCommentsOfBlog(int blogId);
        public Task<bool> UpdateCommentForUserDeletion(ApplicationUser user);
    }
}
