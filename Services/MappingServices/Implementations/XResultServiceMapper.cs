using BlogApp.Models.Response;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Services.MappingServices.Implementations
{
    public class XResultServiceMapper : IXResultServiceMapper
    {
        public ServiceResult Map(IdentityResult result)
        {
            if(result.Succeeded)
            {
                return ServiceResult.Success();
            }
            else
            {
                IEnumerable<Message> messages = new List<Message>();
                foreach (var item in result.Errors)
                {
                    messages.Append(new Message { Code = item.Code, Description = item.Description });
                }
                return ServiceResult.Failed(messages.ToArray());
            }
        }

        public ServiceResult Map(ValidationResult result)
        {
            return result.Succeeded == true ?
                ServiceResult.Success() : 
                ServiceResult.Failed(result.Messages.ToArray());
        }
    }
}
