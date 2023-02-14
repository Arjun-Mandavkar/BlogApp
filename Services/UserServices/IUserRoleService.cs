using BlogApp.Models.Dtos;
using BlogApp.Models.Response;

namespace BlogApp.Services.UserServices
{
    public interface IUserRoleService
    {
        Task<ServiceResult> AssignRole(int userId, RoleEnum role);
    }
}
