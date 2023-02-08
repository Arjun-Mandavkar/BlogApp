using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class BlogOwner
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int BlogId { get; set; }
    }
}
