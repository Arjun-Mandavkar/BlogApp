using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Repositories;
using BlogApp.Services.MappingServices;
using BlogApp.Services.UserServices;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Services.BlogServices.Implementation
{
    public class BlogEditorService : IBlogEditorService
    {
        private IBlogEditorsStore<Blog, ApplicationUser> _blogEditorStore;
        private IUserCrudService _userCrudService;
        private IUserMapper _userMapper;
        private IBlogStore<Blog> _blogStore;
        public BlogEditorService(IBlogEditorsStore<Blog, ApplicationUser> blogEditorStore,
                                 IUserCrudService userCrudService,
                                 IUserMapper userMapper,
                                 IBlogStore<Blog> blogStore)
        {
            _blogEditorStore = blogEditorStore;
            _userCrudService = userCrudService;
            _userMapper = userMapper;
            _blogStore = blogStore;
        }

        public async Task<IEnumerable<UserInfoDto>> GetAll(int blogId)
        {
            IEnumerable<int> ids = await _blogEditorStore.Get(blogId);

            IEnumerable<ApplicationUser> users = new List<ApplicationUser>();
            foreach (int id in ids)
                users.Append(await _userCrudService.FindById(id.ToString()));

            IEnumerable<UserInfoDto> result = new List<UserInfoDto>();
            foreach (ApplicationUser user in users)
                result.Append(_userMapper.Map(user));

            return result;
        }

        public async Task<ServiceResult> Assign(int blogId, int userId)
        {
            ApplicationUser user = await _userCrudService.FindById(userId.ToString());
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
            ApplicationUser user = await _userCrudService.FindById(userId.ToString());
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
