using BlogApp.Models.Dtos;

namespace BlogApp.Utilities.JwtUtils
{
    public interface IAuthUtils
    {
        public Task<string> GenerateToken(UserInfoDto user);
        public Task<string> GetLoggedInUserId();
    }
}
