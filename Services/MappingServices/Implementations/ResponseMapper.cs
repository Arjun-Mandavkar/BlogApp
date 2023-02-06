using BlogApp.Models.Dtos;
using BlogApp.Models.Response;

namespace BlogApp.Services.MappingServices.Implementations
{
    public class ResponseMapper : IResponseMapper
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
                                       new Message { Code = "StackTrace", Description = ex.ToString() }
            }
        };

        public ApiResponse Map(UserInfoDto user) => new ApiResponse
        {
            IsSuccess = true,
            Data = new List<UserInfoDto> { user }
        };

        /// <summary>
        /// Used only for returning 'ERROR' message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Information about failure</returns>
        public ApiResponse Map(Message message) => new ApiResponse
        {
            IsSuccess = false,
            Data = new List<Message> { message }
        };

        /// <summary>
        /// Used for returning Success or Information message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ApiResponse MapMessage(Message message) => new ApiResponse
        {
            IsSuccess = true,
            Data = new List<Message> { message }
        };

        /// <summary>
        /// Used only for returning 'ERROR' messages
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Information about failure</returns>
        public ApiResponse Map(IEnumerable<Message> messages) => new ApiResponse
        {
            IsSuccess = false,
            Data =  messages
        };

        /// <summary>
        /// Used for returning Success or Information messages
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ApiResponse MapMessages(IEnumerable<Message> messages) => new ApiResponse
        {
            IsSuccess = true,
            Data = messages
        };

        public ApiResponse Map(ServiceResult result) => new ApiResponse
        {
            IsSuccess = result.Succeeded,
            Data = result.Messages
        };
    }
}
