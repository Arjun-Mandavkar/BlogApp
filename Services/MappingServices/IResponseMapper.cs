using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using Boolean = BlogApp.Models.Response.Boolean;

namespace BlogApp.Services.MappingServices
{
    public interface IResponseMapper : IMapper<Exception, ApiResponse<Message>>,
                                       IMapper<UserInfoDto, ApiResponse<UserInfoDto>>,
                                       IMapper<Message, ApiResponse<Message>>,
                                       IMapper<ServiceResult, ApiResponse<List<Message>>>,
                                       IMapper<Boolean, ApiResponse<Boolean>>,
                                       IMapper<List<BlogCommentDto>, ApiResponse<List<BlogCommentDto>>>,
                                       IMapper<BlogAuthorsDto, ApiResponse<BlogAuthorsDto>>,
                                       IMapper<BlogDto, ApiResponse<BlogDto>>,
                                       IMapper<List<BlogDto>, ApiResponse<List<BlogDto>>>
    {
        /// <summary>
        /// Used only for returning 'ERROR' messages
        /// </summary>
        /// <param name="messages"></param>
        /// <returns>Information about failure</returns>
        public ApiResponse<List<Message>> Map(params Message[] messages);

        /// <summary>
        /// Used to convert exception to dev env api response
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public ApiResponse<List<Message>> MapDev(Exception exception);

        /// <summary>
        /// Used for returning Success or Information message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ApiResponse<Message> MapMessage(Message message);

        /// <summary>
        /// Used for returning Success or Information messages
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        public ApiResponse<List<Message>> MapMessages(params Message[] messages);
    }
}
