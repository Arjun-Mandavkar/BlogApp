using BlogApp.Models.Response;

namespace BlogApp.Models.Dtos
{
    public class UserInfoDto : IResponseData
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
