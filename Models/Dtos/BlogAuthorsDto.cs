using BlogApp.Models.Response;

namespace BlogApp.Models.Dtos
{
    public class BlogAuthorsDto : IResponseData
    {
        public IEnumerable<UserInfoDto> Owners { get; set; }
        public IEnumerable<UserInfoDto> Editors { get; set; }
    }
}
