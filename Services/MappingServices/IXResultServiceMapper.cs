using BlogApp.Models.Response;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Services.MappingServices
{
    public interface IXResultServiceMapper : IMapper<IdentityResult, ServiceResult>,
                                             IMapper<ValidationResult, ServiceResult>
    {
    }
}
