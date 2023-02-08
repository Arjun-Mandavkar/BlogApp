using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BlogApp.Models
{
    public class BlogComment
    {
        [Key]        
        public int Id { get; set; }
        [Required]
        public int BlogId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Text { get; set; } = string.Empty;
        [Required]
        public DateTime TimeStamp { get; set; } = DateTime.Now;
    }
}
