using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Services.UserServices;
using BloggingApplication.Models.Dtos;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Services.MappingServices.Implementations
{
    public class UserMapper : IUserMapper
    {
        private IUserAuthService _userAuthService;
        public PasswordHasher<ApplicationUser> _hasher;
        public UserMapper(IUserAuthService userAuthService, PasswordHasher<ApplicationUser> hasher)
        {
            _userAuthService = userAuthService;
            _hasher = hasher;
        }

        public UserInfoDto Map(ApplicationUser user) => new UserInfoDto
        {
            Email = user.Email,
            Id = user.Id,
            Role = user.Role.ToString(),
            Name = user.Name
        };
        public async Task<AuthUserInfoDto> MapAuthUser(ApplicationUser user) => new AuthUserInfoDto
        {
            Email = user.Email,
            Id = user.Id,
            Role = user.Role.ToString(),
            Name = user.Name,
            Token = await _userAuthService.GenerateToken(user)
        };

        public ApplicationUser Map(RegisterUserDto user) => new ApplicationUser
        {
            Email = user.Email,
            Name = user.Name,
            UserName = user.Email,
            Role = user.Role,
            PasswordHash = _hasher.HashPassword(new ApplicationUser(), user.Password)
        };

        
    }
}
