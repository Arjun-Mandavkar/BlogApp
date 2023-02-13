using BlogApp.Models;
using BlogApp.Models.Dtos;
using BloggingApplication.Models.Dtos;

namespace BlogApp.Utilities.MappingUtils.Implementations
{
    public class UserMapper : IUserMapper
    {
        public UserInfoDto Map(ApplicationUser user) => new UserInfoDto
        {
            Email = user.Email,
            Id = user.Id,
            Role = user.Role.ToString(),
            Name = user.Name
        };
        public AuthUserInfoDto Map(UserInfoDto user, string token) => new AuthUserInfoDto
        {
            Email = user.Email,
            Id = user.Id,
            Role = user.Role.ToString(),
            Name = user.Name,
            Token = token
        };

        public ApplicationUser Map(RegisterUserDto user, string passwordHash) => new ApplicationUser
        {
            Email = user.Email,
            Name = user.Name,
            UserName = user.Email,
            Role = user.Role,
            PasswordHash = passwordHash
        };

        public RegisterUserDto MapExt(ApplicationUser user) => new RegisterUserDto
        {
            Email = user.Email,
            Name = user.Name,
            Role = user.Role
        };
    }
}
