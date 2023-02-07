using BlogApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogApp.Services.UserServices.Implementations
{
    public class UserAuthService : IUserAuthService
    {
        private readonly IConfiguration _configuration;
        private IHttpContextAccessor _httpContextAccessor;
        private IUserCrudService _userCrudService;

        public UserAuthService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IUserCrudService userCrudService)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _userCrudService = userCrudService;
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

        public async Task<ApplicationUser> GetLoggedInUser()
        {
            string userId = _httpContextAccessor.HttpContext.User.Claims
                            .FirstOrDefault(c => c.Type == "Id").Value;

            ApplicationUser user = await _userCrudService.FindById(userId.ToString());
            return user;
        }
    }
}
