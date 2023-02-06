using BlogApp.Models.Response;

namespace BlogApp.Services.BlogServices
{
    public interface IBlogLikeService
    {
        public Task<bool> IsLiked(int blogId);
        public Task<ServiceResult> LikeBlog(int blogId);
        public Task<ServiceResult> DeleteLikeBlog(int blogId);
    }
}
