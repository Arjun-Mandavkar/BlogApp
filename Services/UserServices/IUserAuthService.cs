using BlogApp.Models.Dtos;
using BlogApp.Models.Response;

namespace BlogApp.Services.UserServices
{
    public interface IUserAuthService
    {
        public Task<ServiceResult> VerifyPassword(UserInfoDto user, string password);
    }
}
