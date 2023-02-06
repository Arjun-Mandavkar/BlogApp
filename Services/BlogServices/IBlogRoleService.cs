using BlogApp.Models.Dtos;
using BlogApp.Models.Response;

namespace BlogApp.Services.BlogServices
{
    public interface IBlogRoleService
    {
        public Task<ServiceResult> AssignRoles(BlogRoleDto dto);
        public Task<ServiceResult> RevokeRoles(BlogRoleDto dto);
    }
}
