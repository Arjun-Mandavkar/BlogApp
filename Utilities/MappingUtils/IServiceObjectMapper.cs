using BlogApp.Models.Dtos;
using BlogApp.Models.ServiceObjects;
using BlogApp.Models;
using BloggingApplication.Models.Dtos;

namespace BlogApp.Utilities.MappingUtils
{
    public interface IServiceObjectMapper : IMapper<Blog, BlogServiceObject>,
                                            IMapper<BlogServiceObject, BlogDto>,
                                            IMapper<BlogComment, BlogCommentServiceObject>,
                                            IMapper<BlogCommentServiceObject, BlogCommentDto>,
                                            IMapper<ApplicationUser, UserServiceObject>,
                                            IMapper<UserServiceObject, UserInfoDto>
    {
        AuthUserInfoDto Map(UserServiceObject user, string token);
    }
}
