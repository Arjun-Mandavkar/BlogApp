using BlogApp.Models;
using BlogApp.Models.Response;
using BlogApp.Models.ServiceObjects;
using BlogApp.Repositories;
using BlogApp.Utilities.MappingUtils;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Services.BlogServices.Implementation
{
    public class BlogOwnerService : IBlogOwnerService
    {
        private IBlogOwnersStore<BlogOwner> _blogOwnerStore;
        private IMyUserStore _userStore;
        private IBlogMapper _blogMapper;
        private IBlogStore<Blog> _blogStore;
        private IServiceObjectMapper _serviceObjectMapper;
        public BlogOwnerService(IBlogOwnersStore<BlogOwner> blogOwnerStore,
                                IMyUserStore userStore,
                                IBlogMapper blogMapper,
                                IBlogStore<Blog> blogStore,
                                IServiceObjectMapper serviceObjectMapper)
        {
            _blogOwnerStore = blogOwnerStore;
            _userStore = userStore;
            _blogMapper = blogMapper;
            _blogStore = blogStore;
            _serviceObjectMapper = serviceObjectMapper;
        }

        public async Task<IEnumerable<UserServiceObject>> GetAll(int blogId)
        {
            IEnumerable<int> ids = await _blogOwnerStore.Get(blogId);

            IEnumerable<ApplicationUser> users = new List<ApplicationUser>();
            foreach (int id in ids)
                users.Append(await _userStore.FindByIdAsync(id.ToString(), CancellationToken.None));

            IEnumerable<UserServiceObject> result = new List<UserServiceObject>();
            foreach (ApplicationUser user in users)
                result.Append(_serviceObjectMapper.Map(user));

            return result;
        }

        public async Task<ServiceResult> Assign(int blogId, int userId)
        {
            ApplicationUser user = await _userStore.FindByIdAsync(userId.ToString(), CancellationToken.None);
            if (user == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "User not found." });

            Blog blog = await _blogStore.GetByIdAsync(blogId);
            if (blog == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "Blog not found." });

            bool isSpecifiedUserOwner = await _blogOwnerStore.IsOwner(user.Id, blog.Id);

            BlogUserObject blogUser = new BlogUserObject { Blog = blog, User = user };

            if (!isSpecifiedUserOwner)
            {
                //Insert entry in owner table
                IdentityResult result = await _blogOwnerStore.AssignOwner(_blogMapper.Map(blogUser));
                if (!result.Succeeded)
                    return ServiceResult.Failed(new Message { Code = "Error", Description = "Assigning user as owner failed." });

                return ServiceResult.Success(new Message { Code = "Message", Description = "User assigned as owner successfully." });
            }
            else
                return ServiceResult.Success(new Message { Code = "Message", Description = "User is already an owner." });
        }

        public async Task<ServiceResult> Revoke(int blogId, int userId)
        {
            ApplicationUser user = await _userStore.FindByIdAsync(userId.ToString(), CancellationToken.None);
            if (user == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "User not found." });

            Blog blog = await _blogStore.GetByIdAsync(blogId);
            if (blog == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "Blog not found." });

            bool isSpecifiedUserOwner = await _blogOwnerStore.IsOwner(user.Id, blog.Id);

            if (isSpecifiedUserOwner)
            {
                //Remove entry from owner table
                IdentityResult result = await _blogOwnerStore.RevokeOwner(blog.Id, user.Id);
                if (!result.Succeeded)
                    return ServiceResult.Failed(new Message { Code = "Error", Description = "Removing user from owner role failed." });

                return ServiceResult.Success(new Message { Code = "Message", Description = "User revoked as owner successfully." });
            }
            else
                return ServiceResult.Success(new Message { Code = "Message", Description = "User is not an owner." });
        }
    }
}
