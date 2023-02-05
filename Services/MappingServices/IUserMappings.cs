using BlogApp.Models;
using BlogApp.Models.Dtos;

namespace BlogApp.Services.MappingServices
{
    public interface IUserMappings : IMapper<ApplicationUser, UserInfoDto>
    {
    }
}
