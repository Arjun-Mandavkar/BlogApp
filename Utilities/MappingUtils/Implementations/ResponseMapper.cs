using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BloggingApplication.Models.Dtos;
using Boolean = BlogApp.Models.Response.Boolean;

namespace BlogApp.Utilities.MappingUtils.Implementations
{
    public class ResponseMapper : IResponseMapper
    {
        public ApiResponse<Message> Map(Exception ex) => new ApiResponse<Message>
        {
            IsSuccess = false,
            Data = new Message { Code = "Error", Description = ex.Message }
        };

        public ApiResponse<IEnumerable<Message>> MapDev(Exception ex) => new ApiResponse<IEnumerable<Message>>
        {
            IsSuccess = false,
            Data = new List<Message> { new Message { Code = "Error", Description = ex.Message },
                                       new Message { Code = "StackTrace", Description = ex.ToString() }
            }
        };

        public ApiResponse<UserInfoDto> Map(UserInfoDto user) => new ApiResponse<UserInfoDto>
        {
            IsSuccess = true,
            Data = user
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

        public ApiResponse<IEnumerable<Message>> Map(params Message[] messages) => new ApiResponse<IEnumerable<Message>>
        {
            IsSuccess = false,
            Data = messages.ToList()
        };

        public ApiResponse<IEnumerable<Message>> MapMessages(params Message[] messages) => new ApiResponse<IEnumerable<Message>>
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
            Data = result
        };

        public ApiResponse<IEnumerable<BlogCommentDto>> Map(IEnumerable<BlogCommentDto> comments) => new ApiResponse<IEnumerable<BlogCommentDto>>
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

        public ApiResponse<IEnumerable<BlogDto>> Map(IEnumerable<BlogDto> source) => new ApiResponse<IEnumerable<BlogDto>>
        {
            IsSuccess = true,
            Data = source
        };

        public ApiResponse<AuthUserInfoDto> Map(AuthUserInfoDto user) => new ApiResponse<AuthUserInfoDto>
        {
            IsSuccess = true,
            Data = user
        };
    }
}
