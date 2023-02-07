using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class BlogOwner
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int BlogId { get; set; }
        [Required]
        public string OwnerName { get; set; } = string.Empty;
        [Required]
        public bool IsOwnerExists { get; set; } = true;
    }
}
