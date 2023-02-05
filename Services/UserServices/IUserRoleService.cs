using BlogApp.Models;
using BlogApp.Models.Response;

namespace BlogApp.Services.UserServices
{
    public interface IUserRoleService
    {
        Task<ServiceResult> ChangeRole(int userId, UserRole role);
    }
}
