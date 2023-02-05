using BlogApp.Models;

namespace BlogApp.MappingServices
{
    public interface IExceptionResponseMapper : IMapper<Exception, ApiResponse>
    {
        public ApiResponse MapDev(Exception exception);
    }
}
