using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Models.ServiceObjects;

namespace BlogApp.Services.UserServices
{
    public interface IUserCrudService
    {
        public Task<UserServiceObject> FindByEmail(string email);
        public Task<UserServiceObject> FindById(string userId);
        public Task<UserServiceObject> CreateUser(RegisterUserDto dto);
        public Task<ServiceResult> UpdateUser(RegisterUserDto dto);
        public Task<ServiceResult> DeleteUser(string userId);
        public Task<ServiceResult> SoftDeleteUser(string email);
    }
}
