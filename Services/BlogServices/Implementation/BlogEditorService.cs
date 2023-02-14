using BlogApp.Models;
using BlogApp.Models.Response;
using BlogApp.Models.ServiceObjects;
using BlogApp.Repositories;
using BlogApp.Utilities.MappingUtils;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Services.BlogServices.Implementation
{
    public class BlogEditorService : IBlogEditorService
    {
        private IBlogEditorsStore<Blog, ApplicationUser> _blogEditorStore;
        private IMyUserStore _userStore;
        private IBlogStore<Blog> _blogStore;
        private IServiceObjectMapper _serviceObjectMapper;
        public BlogEditorService(IBlogEditorsStore<Blog, ApplicationUser> blogEditorStore,
                                 IMyUserStore userStore,
                                 IBlogStore<Blog> blogStore,
                                 IServiceObjectMapper serviceObjectMapper)
        {
            _blogEditorStore = blogEditorStore;
            _userStore = userStore;
            _blogStore = blogStore;
            _serviceObjectMapper = serviceObjectMapper;
        }

        public async Task<IEnumerable<UserServiceObject>> GetAll(int blogId)
        {
            IEnumerable<int> ids = await _blogEditorStore.Get(blogId);

            IEnumerable<ApplicationUser> users = new List<ApplicationUser>();
            foreach (int id in ids)
            {
                users.Append(await _userStore.FindByIdAsync(id.ToString(), CancellationToken.None));
            }

            IEnumerable<UserServiceObject> result = new List<UserServiceObject>();
            foreach(ApplicationUser user in users)
            {
                result.Append(_serviceObjectMapper.Map(user));
            }
            
            return result;
        }

        public async Task<ServiceResult> Assign(int blogId, int userId)
        {
            ApplicationUser user = await _userStore.FindByIdAsync(userId.ToString(), CancellationToken.None);
            if (user == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "User not found." });

            Blog blog = await _blogStore.GetByIdAsync(blogId);
            if(blog == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "Blog not found." });

            bool isSpecifiedUserEditor = await _blogEditorStore.IsEditor(blog, user);

            if (!isSpecifiedUserEditor)
            {
                //Insert entry into editor table
                IdentityResult result = await _blogEditorStore.AssignEditor(blog, user);
                if (!result.Succeeded)
                    return ServiceResult.Failed(new Message { Code = "Error", Description = "An error occured while assigning editor role." });
                return ServiceResult.Success(new Message { Code = "Message", Description = "User assigned as editor successfully." });
            }
            else
            {
                return ServiceResult.Success(new Message { Code = "Message", Description = "User is already assigned as an editor." });
            }
        }

        public async Task<ServiceResult> Revoke(int blogId, int userId)
        {
            ApplicationUser user = await _userStore.FindByIdAsync(userId.ToString(), CancellationToken.None);
            if (user == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "User not found." });

            Blog blog = await _blogStore.GetByIdAsync(blogId);
            if (blog == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "Blog not found." });

            bool isSpecifiedUserEditor = await _blogEditorStore.IsEditor(blog, user);

            if (isSpecifiedUserEditor)
            {
                //Remove entry into editor table
                IdentityResult result = await _blogEditorStore.RevokeEditor(blog, user);
                if (!result.Succeeded)
                    return ServiceResult.Failed(new Message { Code = "Error", Description = "An error occured while revoking editor role." });
                return ServiceResult.Success(new Message { Code = "Message", Description = "User revoked as editor successfully." });
            }
            else
            {
                return ServiceResult.Success(new Message { Code = "Message", Description = "User is not an editor." });
            }
        }
    }
}
