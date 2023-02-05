using BlogApp.Models;

namespace BlogApp.MappingServices
{
    public class ExceptionToResponseMapper : IMapper<Exception, ApiResponse>
    {
        public ApiResponse Map(Exception ex) => new ApiResponse
        {
            IsSuccess = false,
            Data = new List<Message> { new Message { Code = "Error", Description = ex.Message } }
        };
    }
}
