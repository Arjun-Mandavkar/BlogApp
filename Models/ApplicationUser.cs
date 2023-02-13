using BlogApp.Models.Dtos;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Name { get; set; } = String.Empty;
        public RoleEnum Role { get; set; }
    }
}
