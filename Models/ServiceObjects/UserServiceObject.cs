using BlogApp.Models.Dtos;

namespace BlogApp.Models.ServiceObjects
{
    public class UserServiceObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public RoleEnum Role { get; set; }
        public string PasswordHash { get; set; } = String.Empty;
    }
}
