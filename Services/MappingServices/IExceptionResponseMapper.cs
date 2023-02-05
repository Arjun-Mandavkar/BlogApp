using BlogApp.Models.Response;

namespace BlogApp.Services.MappingServices
{
    public interface IExceptionResponseMapper : IMapper<Exception, ApiResponse>
    {
        public ApiResponse MapDev(Exception exception);
    }
}
