using BlogApp.Models;
using BlogApp.Models.Dtos;
using BloggingApplication.Models.Dtos;

namespace BlogApp.Utilities.MappingUtils
{
    public interface IUserMapper : IMapper<ApplicationUser, UserInfoDto>
    {
        public AuthUserInfoDto Map(UserInfoDto user, string token);
        public ApplicationUser Map(RegisterUserDto user, string passwordHash);
        public RegisterUserDto MapExt(ApplicationUser user);
    }
}
