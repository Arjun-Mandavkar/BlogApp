using BlogApp.Models.Response;
using BlogApp.Models.ServiceObjects;

namespace BlogApp.Services.BlogServices
{
    public interface IBlogRoleService
    {
        public Task<IEnumerable<UserServiceObject>> GetAll(int blogId);
        public Task<ServiceResult> Assign(int blogId, int userId);
        public Task<ServiceResult> Revoke(int blogId, int userId);
    }
}
