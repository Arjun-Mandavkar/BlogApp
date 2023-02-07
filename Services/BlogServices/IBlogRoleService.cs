using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Models;

namespace BlogApp.Services.BlogServices
{
    public interface IBlogRoleService
    {
        public Task<IEnumerable<UserInfoDto>> GetAll(int blogId);
        public Task<ServiceResult> Assign(Blog blog, ApplicationUser user);
        public Task<ServiceResult> Revoke(Blog blog, ApplicationUser user);
    }
}
