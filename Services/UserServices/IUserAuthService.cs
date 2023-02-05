using BlogApp.Models;

namespace BlogApp.Services.UserServices
{
    public interface IUserAuthService
    {
        public Task<bool> IsUserExist(string email);
        public Task<bool> IsPasswordCorrect(ApplicationUser user, string password);
    }
}
