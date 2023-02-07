using System.Xml.Linq;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using Boolean = BlogApp.Models.Response.Boolean;

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

        public ApiResponse MapMessage(Message message) => new ApiResponse
        {
            IsSuccess = true,
            Data = new List<Message> { message }
        };

        public ApiResponse Map(params Message[] messages) => new ApiResponse
        {
            IsSuccess = false,
            Data =  messages
        };

        public ApiResponse MapMessages(params Message[] messages) => new ApiResponse
        {
            IsSuccess = true,
            Data = messages
        };

        public ApiResponse Map(ServiceResult result) => new ApiResponse
        {
            IsSuccess = result.Succeeded,
            Data = result.Messages
        };

        public ApiResponse Map(Boolean result) => new ApiResponse
        {
            IsSuccess = true,
            Data = new List<Boolean> { result }
        };

        public ApiResponse Map(IEnumerable<BlogCommentDto> comments) => new ApiResponse
        {
            IsSuccess = true,
            Data = comments
        };

        public ApiResponse Map(BlogAuthorsDto dto) => new ApiResponse
        {
            IsSuccess = true,
            Data = new List<BlogAuthorsDto> { dto }
        };
    }
}
