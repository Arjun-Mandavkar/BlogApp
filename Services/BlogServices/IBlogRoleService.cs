using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;

namespace BlogApp.Services.BlogServices
{
    public interface IBlogRoleService
    {
        public Task<IEnumerable<UserInfoDto>> GetAll(int blogId);
        public Task<ServiceResult> Assign(int blogId, int userId);
        public Task<ServiceResult> Revoke(int blogId, int userId);
    }
}
