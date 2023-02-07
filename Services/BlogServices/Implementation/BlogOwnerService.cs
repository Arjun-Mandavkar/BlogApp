using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Repositories;
using BlogApp.Services.MappingServices;
using BlogApp.Services.UserServices;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Services.BlogServices.Implementation
{
    public class BlogOwnerService : IBlogOwnerStore
    {
        private IBlogOwnersStore<BlogOwner> _blogOwnerStore;
        private IUserCrudService _userCrudService;
        private IUserMapper _userMapper;
        private IBlogMapper _blogMapper;
        public BlogOwnerService(IBlogOwnersStore<BlogOwner> blogOwnerStore,
                                IUserCrudService userCrudService,
                                IUserMapper userMapper,
                                IBlogMapper blogMapper)
        {
            _blogOwnerStore = blogOwnerStore;
            _userCrudService = userCrudService;
            _userMapper = userMapper;
            _blogMapper = blogMapper;
        }

        public async Task<IEnumerable<UserInfoDto>> GetAll(Blog blog)
        {
            IEnumerable<int> ids = await _blogOwnerStore.Get(blog.Id);

            IEnumerable<ApplicationUser> users = new List<ApplicationUser>();
            foreach (int id in ids)
                users.Append(await _userCrudService.FindById(id.ToString()));

            IEnumerable<UserInfoDto> result = new List<UserInfoDto>();
            foreach (ApplicationUser user in users)
                result.Append(_userMapper.Map(user));

            return result;
        }

        public async Task<ServiceResult> Assign(Blog blog, ApplicationUser user)
        {
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

        public async Task<ServiceResult> Revoke(Blog blog, ApplicationUser user)
        {
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

        public async Task<bool> UpdateOwnerEntryForUserDeletion(int userId)
        {
            IdentityResult result = await _blogOwnerStore.SetIsOwnerExistsFalse(userId);
            if (result.Succeeded)
                return true;
            else
                return false;
        }
    }
}
