using Microsoft.AspNetCore.Identity;

namespace BlogApp.Models
{
    public class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
