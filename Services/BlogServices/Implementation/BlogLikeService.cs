using System.Transactions;
using BlogApp.Models;
using BlogApp.Models.Response;
using BlogApp.Repositories;
using BlogApp.Services.UserServices;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Services.BlogServices.Implementation
{
    public class BlogLikeService : IBlogLikeService
    {
        private IBlogStore<Blog> _blogStore;
        private IUserAuthService _userAuthService;
        private IBlogLikesStore<Blog, ApplicationUser> _blogLikesStore;
        public BlogLikeService(IBlogStore<Blog> blogStore,
                               IUserAuthService userAuthService,
                               IBlogLikesStore<Blog, ApplicationUser> blogLikesStore)
        {
            _blogStore = blogStore;
            _userAuthService = userAuthService;
            _blogLikesStore = blogLikesStore;
        }

        public async Task<bool> IsLiked(int blogId)
        {
            //Fetch detached blog object
            Blog blog = await _blogStore.GetByIdAsync(blogId);
            if (blog == null) return false;

            //Fetch logged in user details
            ApplicationUser user = await _userAuthService.GetLoggedInUser();

            return await _blogLikesStore.IsLikedAsync(blog, user);
        }

        public async Task<ServiceResult> LikeBlog(int blogId)
        {
            //Fetch detached blog object
            Blog blog = await _blogStore.GetByIdAsync(blogId);
            if (blog == null)
                return ServiceResult.Failed(new Message { Code = "Message", Description = "Blog not found" });

            //Fetch logged in user details
            ApplicationUser user = await _userAuthService.GetLoggedInUser();

            bool res = await _blogLikesStore.IsLikedAsync(blog, user);
            if (res)
                return ServiceResult.Failed(new Message { Code = "Message", Description = "Already Liked" });
            else
            {
                using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    //Insert Like
                    IdentityResult result = await _blogLikesStore.LikeAsync(blog, user);
                    if (!result.Succeeded)
                        return ServiceResult.Failed(new Message { Code = "Message", Description = "Blog like failed!" });

                    result = await _blogStore.IncrementLike(blog.Id);
                    if (!result.Succeeded)
                        return ServiceResult.Failed(new Message { Code = "Error", Description = "Blog like increment failed!" });

                    tx.Complete();
                }
                return ServiceResult.Success(new Message { Code = "Message", Description = "Blog liked successfully!" });
            }
        }

        public async Task<ServiceResult> DeleteLikeBlog(int blogId)
        {
            //Fetch detached blog object
            Blog blog = await _blogStore.GetByIdAsync(blogId);
            if (blog == null)
                return ServiceResult.Failed(new Message { Code = "Message", Description = "Blog not found" });

            //Fetch logged in user details
            ApplicationUser user = await _userAuthService.GetLoggedInUser();

            bool res = await _blogLikesStore.IsLikedAsync(blog, user);
            if (res)
                return ServiceResult.Failed(new Message { Code = "Message", Description = "Already Liked" });
            else
            {
                using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    //Insert Like
                    IdentityResult result = await _blogLikesStore.UndoLikeAsync(blog, user);
                    if (!result.Succeeded)
                        return ServiceResult.Failed(new Message { Code = "Message", Description = "Blog undo like failed!" });

                    result = await _blogStore.DecrementLike(blog.Id);
                    if (!result.Succeeded)
                        return ServiceResult.Failed(new Message { Code = "Error", Description = "Blog like decrement failed!" });

                    tx.Complete();
                }
                return ServiceResult.Success(new Message { Code = "Message", Description = "Blog like removed successfully!" });
            }
        }
    }
}
