using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using Boolean = BlogApp.Models.Response.Boolean;

namespace BlogApp.Services.MappingServices
{
    public interface IResponseMapper : IMapper<Exception, ApiResponse>,
                                       IMapper<UserInfoDto, ApiResponse>,
                                       IMapper<Message, ApiResponse>,
                                       IMapper<ServiceResult, ApiResponse>,
                                       IMapper<Boolean, ApiResponse>,
                                       IMapper<IEnumerable<BlogCommentDto>, ApiResponse>,
                                       IMapper<BlogAuthorsDto, ApiResponse>
    {
        /// <summary>
        /// Used only for returning 'ERROR' messages
        /// </summary>
        /// <param name="messages"></param>
        /// <returns>Information about failure</returns>
        public ApiResponse Map(params Message[] messages);

        /// <summary>
        /// Used to convert exception to dev env api response
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public ApiResponse MapDev(Exception exception);

        /// <summary>
        /// Used for returning Success or Information message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ApiResponse MapMessage(Message message);

        /// <summary>
        /// Used for returning Success or Information messages
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        public ApiResponse MapMessages(params Message[] messages);
    }
}
