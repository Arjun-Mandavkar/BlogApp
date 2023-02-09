using BlogApp.Models.Response;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Utilities.MappingUtils
{
    public interface IXResultServiceMapper : IMapper<IdentityResult, ServiceResult>,
                                             IMapper<ValidationResult, ServiceResult>
    {
    }
}
