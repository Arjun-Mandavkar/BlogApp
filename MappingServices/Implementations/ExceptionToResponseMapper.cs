using BlogApp.Models.Response;

namespace BlogApp.MappingServices.Implementations
{
    public class ExceptionToResponseMapper : IExceptionResponseMapper
    {
        public ApiResponse Map(Exception ex) => new ApiResponse
        {
            IsSuccess = false,
            Data = new List<Message> { new Message { Code = "Error", Description = ex.Message } }
        };

        public ApiResponse MapDev(Exception ex) => new ApiResponse
        {
            IsSuccess = false,
            Data = new List<Message> { new Message { Code = "Error", Description = ex.Message },
                                       new Message{Code = "StackTrace", Description = ex.ToString()}
            }
        };
    }
}
