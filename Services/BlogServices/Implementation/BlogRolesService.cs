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
        public BlogRolesService(IBlogCrudService blogCrudService,
                                IUserCrudService userCrudService)
        {
            _blogCrudService = blogCrudService;
            _userCrudService = userCrudService;
        }

        public async Task<ServiceResult> AssignRoles(BlogRoleDto dto)
        {
            Blog blog = await _blogCrudService.Get(dto.BlogId);
            if (blog == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "Blog not found." });

            ApplicationUser user = await _userCrudService.FindById(dto.UserId.ToString());
            if (user == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "User not found." });
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> RevokeRoles(BlogRoleDto dto)
        {
            Blog blog = await _blogCrudService.Get(dto.BlogId);
            if (blog == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "Blog not found." });

            ApplicationUser user = await _userCrudService.FindById(dto.UserId.ToString());
            if (user == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "User not found." });
            throw new NotImplementedException();
        }
    }
}
