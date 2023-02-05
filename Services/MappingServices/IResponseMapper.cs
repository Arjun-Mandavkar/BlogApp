using BlogApp.Models.Dtos;
using BlogApp.Models.Response;

namespace BlogApp.Services.MappingServices
{
    public interface IResponseMapper : IMapper<Exception, ApiResponse>,
                                       IMapper<UserInfoDto, ApiResponse>,
                                       IMapper<Message, ApiResponse>,
                                       IMapper<IEnumerable<Message>, ApiResponse>
    {
        public ApiResponse MapDev(Exception exception);
        public ApiResponse MapMessage(Message message);
        public ApiResponse MapMessages(IEnumerable<Message> messages);
    }
}
