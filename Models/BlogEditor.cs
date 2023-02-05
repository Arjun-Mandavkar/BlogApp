using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class BlogEditor
    {
        [Required]
        public int BlogId { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
