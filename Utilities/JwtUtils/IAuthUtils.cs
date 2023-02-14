using System.Security.Claims;

namespace BlogApp.Utilities.JwtUtils
{
    public interface IAuthUtils
    {
        public Task<string> GenerateToken(IEnumerable<Claim> claims);

        public Task<string> GetLoggedInUserId();
    }
}
