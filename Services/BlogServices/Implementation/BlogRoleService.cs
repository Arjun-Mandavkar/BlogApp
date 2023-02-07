using BlogApp.Models.Dtos;
using BlogApp.Models.Response;

namespace BlogApp.Services.BlogServices.Implementation
{
    public class BlogRoleService : IBlogRolesService
    {
        public Task<ServiceResult> AssignRoles(BlogRoleDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> RevokeRoles(BlogRoleDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
