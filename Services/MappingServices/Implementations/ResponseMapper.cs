using System.Xml.Linq;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using Boolean = BlogApp.Models.Response.Boolean;

namespace BlogApp.Services.MappingServices.Implementations
{
    public class ResponseMapper : IResponseMapper
    {
        public ApiResponse<Message> Map(Exception ex) => new ApiResponse<Message>
        {
            IsSuccess = false,
            Data = new Message { Code = "Error", Description = ex.Message  }
        };

        public ApiResponse<List<Message>> MapDev(Exception ex) => new ApiResponse<List<Message>>
        {
            IsSuccess = false,
            Data = new List<Message> { new Message { Code = "Error", Description = ex.Message },
                                       new Message { Code = "StackTrace", Description = ex.ToString() }
            }
        };

        public ApiResponse<UserInfoDto> Map(UserInfoDto user) => new ApiResponse<UserInfoDto>
        {
            IsSuccess = true,
            Data =  user 
        };

        /// <summary>
        /// Used only for returning 'ERROR' message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Information about failure</returns>
        public ApiResponse<Message> Map(Message message) => new ApiResponse<Message>
        {
            IsSuccess = false,
            Data = message
        };

        public ApiResponse<Message> MapMessage(Message message) => new ApiResponse<Message>
        {
            IsSuccess = true,
            Data = message
        };

        public ApiResponse<List<Message>> Map(params Message[] messages) => new ApiResponse<List<Message>>
        {
            IsSuccess = false,
            Data = messages.ToList()
        };

        public ApiResponse<List<Message>> MapMessages(params Message[] messages) => new ApiResponse<List<Message>>
        {
            IsSuccess = true,
            Data = messages.ToList()
        };

        public ApiResponse<List<Message>> Map(ServiceResult result) => new ApiResponse<List<Message>>
        {
            IsSuccess = result.Succeeded,
            Data = result.Messages.ToList()
        };

        public ApiResponse<Boolean> Map(Boolean result) => new ApiResponse<Boolean>
        {
            IsSuccess = true,
            Data =  result 
        };

        public ApiResponse<List<BlogCommentDto>> Map(List<BlogCommentDto> comments) => new ApiResponse<List<BlogCommentDto>>
        {
            IsSuccess = true,
            Data = comments.ToList()
        };

        public ApiResponse<BlogAuthorsDto> Map(BlogAuthorsDto dto) => new ApiResponse<BlogAuthorsDto>
        {
            IsSuccess = true,
            Data = dto
        };

        public ApiResponse<BlogDto> Map(BlogDto source) => new ApiResponse<BlogDto>
        {
            IsSuccess = true,
            Data = source
        };

        public ApiResponse<List<BlogDto>> Map(List<BlogDto> source) => new ApiResponse<List<BlogDto>>
        {
            IsSuccess = true,
            Data = source
        };
    }
}
