using BlogApp.Models;

namespace BlogApp.Services.UserServices
{
    public interface IUserAuthService
    {
        public Task<string> GenerateToken(ApplicationUser user);
    }
}
