using BlogApp.Models.Response;

namespace BlogApp.MappingServices
{
    public interface IExceptionResponseMapper : IMapper<Exception, ApiResponse>
    {
        public ApiResponse MapDev(Exception exception);
    }
}
