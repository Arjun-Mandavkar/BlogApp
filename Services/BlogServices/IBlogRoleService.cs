using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;

namespace BlogApp.Services.BlogServices
{
    public interface IBlogRoleService
    {
        public Task<IEnumerable<UserInfoDto>> GetAll(Blog blog);
        public Task<ServiceResult> Assign(Blog blog, ApplicationUser user);
        public Task<ServiceResult> Revoke(Blog blog, ApplicationUser user);
    }
}
