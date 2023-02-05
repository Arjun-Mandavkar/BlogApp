using BlogApp.Models;
using BlogApp.Models.Dtos;
using BloggingApplication.Models.Dtos;

namespace BlogApp.Services.MappingServices
{
    public interface IUserMapper : IMapper<ApplicationUser, UserInfoDto>,
                                   IMapper<RegisterUserDto, ApplicationUser>
    {
        Task<AuthUserInfoDto> MapAuthUser(ApplicationUser user);
    }
}
