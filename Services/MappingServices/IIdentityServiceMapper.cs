using BlogApp.Models.Response;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Services.MappingServices
{
    public interface IIdentityServiceMapper : IMapper<IdentityResult, ServiceResult>
    {
    }
}
