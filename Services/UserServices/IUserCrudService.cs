using BlogApp.Models.Dtos;
using BlogApp.Models.Response;

namespace BlogApp.Services.UserServices
{
    public interface IUserCrudService
    {
        public Task<UserInfoDto> FindByEmail(string email);
        public Task<UserInfoDto> FindById(string userId);
        public Task<UserInfoDto> CreateUser(RegisterUserDto dto);
        public Task<ServiceResult> UpdateUser(RegisterUserDto dto);
        public Task<ServiceResult> DeleteUser(string userId);
        public Task<ServiceResult> SoftDeleteUser(string email);
    }
}
