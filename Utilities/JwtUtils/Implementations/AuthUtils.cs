using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogApp.Models.Dtos;
using Microsoft.IdentityModel.Tokens;

namespace BlogApp.Utilities.JwtUtils.Implementations
{
    public class AuthUtils : IAuthUtils
    {
        private readonly IConfiguration _configuration;
        private IHttpContextAccessor _httpContextAccessor;

        public AuthUtils(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GenerateToken(IEnumerable<Claim> myClaims)
        {
            //Generate the token

            //Generate security key
            string secret = _configuration.GetSection("Jwt:Secret").Value;
            byte[] secretBytes = Encoding.UTF8.GetBytes(secret);
            SymmetricSecurityKey key = new SymmetricSecurityKey(secretBytes);

            //Generate credentials for token
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Create jwt token object
            JwtSecurityToken jwtToken = new JwtSecurityToken(
            signingCredentials: creds,
            claims: myClaims,
            expires: DateTime.Now.AddDays(1),
                issuer: _configuration.GetSection("AuthSettings:Issuer").Value,
                audience: _configuration.GetSection("AuthSettings:Audience").Value
            );

            //Generate string of token
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        public async Task<string> GetLoggedInUserId()
        {
            string userId = _httpContextAccessor.HttpContext.User.Claims
                            .FirstOrDefault(c => c.Type == "Id").Value;

            return userId;
        }
    }
}
