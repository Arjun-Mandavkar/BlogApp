using BlogApp.Models;

namespace BlogApp.Utilities.JwtUtils
{
    public interface IAuthUtils
    {
        public Task<string> GenerateToken(ApplicationUser user);
        public Task<string> GetLoggedInUserId();
    }
}
