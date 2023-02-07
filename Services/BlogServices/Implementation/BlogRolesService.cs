using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Services.UserServices;

namespace BlogApp.Services.BlogServices.Implementation
{
    public class BlogRolesService : IBlogRolesService
    {
        private IBlogCrudService _blogCrudService;
        private IUserCrudService _userCrudService;
        private IBlogEditorService _blogEditorService;
        private IBlogOwnerStore _blogOwnerService;
        public BlogRolesService(IBlogCrudService blogCrudService,
                                IUserCrudService userCrudService,
                                IBlogEditorService blogEditorService,
                                IBlogOwnerStore blogOwnerService)
        {
            _blogCrudService = blogCrudService;
            _userCrudService = userCrudService;
            _blogEditorService = blogEditorService;
            _blogOwnerService = blogOwnerService;
        }

        public async Task<ServiceResult> AssignRoles(BlogRoleDto dto)
        {
            Blog blog = await _blogCrudService.Get(dto.BlogId);
            if (blog == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "Blog not found." });

            ApplicationUser user = await _userCrudService.FindById(dto.UserId.ToString());
            if (user == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "User not found." });

            if (dto.Roles.Contains(BlogRoleEnum.OWNER))
                return await _blogOwnerService.Assign(blog, user);
            else if (dto.Roles.Contains(BlogRoleEnum.EDITOR))           //This else is requirement
                return await _blogEditorService.Assign(blog, user);

            return ServiceResult.Success(new Message { Code = "Message", Description = "Roles assigned successfully."});
        }

        public async Task<ServiceResult> RevokeRoles(BlogRoleDto dto)
        {
            Blog blog = await _blogCrudService.Get(dto.BlogId);
            if (blog == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "Blog not found." });

            ApplicationUser user = await _userCrudService.FindById(dto.UserId.ToString());
            if (user == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "User not found." });

            if (dto.Roles.Contains(BlogRoleEnum.OWNER))
                return await _blogOwnerService.Revoke(blog, user);
            if (dto.Roles.Contains(BlogRoleEnum.EDITOR))
                return await _blogEditorService.Revoke(blog, user);

            return ServiceResult.Success(new Message { Code = "Message", Description = "Roles revoked successfully." });
        }
    }
}
