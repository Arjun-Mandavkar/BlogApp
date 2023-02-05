using BlogApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogApp.Services.UserServices.Implementation
{
    public class UserAuthService : IUserAuthService
    {
        private readonly IConfiguration _configuration;
        private PasswordHasher<ApplicationUser> _hasher;
        private IUserCrudService _userCrudService;

        public UserAuthService(IConfiguration configuration, PasswordHasher<ApplicationUser> hasher)
        {
            _configuration = configuration;
            _hasher = hasher;
        }

        public async Task<bool> IsUserExist(string email)
        {
            ApplicationUser user = await _userCrudService.FindByEmail(email);
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> IsPasswordCorrect(ApplicationUser user, string password)
        {
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }

        public async Task<string> GenerateToken(ApplicationUser user)
        {
            //Generate the token

            //Prepare list of claims
            List<Claim> myClaims = new List<Claim>()
                    {
                        new Claim("Id", user.Id.ToString()),
                        new Claim("Email", user.Email),
                        new Claim("Name", user.Name),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                    };

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
    }
}
