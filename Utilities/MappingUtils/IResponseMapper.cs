using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BloggingApplication.Models.Dtos;
using Boolean = BlogApp.Models.Response.Boolean;

namespace BlogApp.Utilities.MappingUtils
{
    public interface IResponseMapper : IMapper<Exception, ApiResponse<Message>>,
                                       IMapper<UserInfoDto, ApiResponse<UserInfoDto>>,
                                       IMapper<AuthUserInfoDto, ApiResponse<AuthUserInfoDto>>,
                                       IMapper<Message, ApiResponse<Message>>,
                                       IMapper<ServiceResult, ApiResponse<List<Message>>>,
                                       IMapper<Boolean, ApiResponse<Boolean>>,
                                       IMapper<IEnumerable<BlogCommentDto>, ApiResponse<IEnumerable<BlogCommentDto>>>,
                                       IMapper<BlogAuthorsDto, ApiResponse<BlogAuthorsDto>>,
                                       IMapper<BlogDto, ApiResponse<BlogDto>>,
                                       IMapper<IEnumerable<BlogDto>, ApiResponse<IEnumerable<BlogDto>>>
    {
        /// <summary>
        /// Used only for returning 'ERROR' messages
        /// </summary>
        /// <param name="messages"></param>
        /// <returns>Information about failure</returns>
        public ApiResponse<IEnumerable<Message>> Map(params Message[] messages);

        /// <summary>
        /// Used to convert exception to dev env api response
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public ApiResponse<IEnumerable<Message>> MapDev(Exception exception);

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
        public ApiResponse<IEnumerable<Message>> MapMessages(params Message[] messages);
    }
}
