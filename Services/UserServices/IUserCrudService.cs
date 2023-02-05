using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;

namespace BlogApp.Services.UserServices
{
    public interface IUserCrudService
    {
        public Task<ApplicationUser> FindByEmail(string email);
        public Task<ApplicationUser> FindById(string userId);
        public Task<ApplicationUser> CreateUser(RegisterUserDto dto);
        public Task<ApplicationUser> UpdateUser(ApplicationUser detachedUser);
        public Task<ServiceResult> DeleteUser(ApplicationUser user);
    }
}
