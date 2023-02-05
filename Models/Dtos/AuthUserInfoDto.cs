using BlogApp.Models.Dtos;

namespace BloggingApplication.Models.Dtos
{
    public class AuthUserInfoDto : UserInfoDto
    {
        public string Token { get; set; } = string.Empty;
    }
}
