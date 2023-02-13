namespace BlogApp.Models
{
    public class BlogComment
    {   
        public int Id { get; set; }
        public int BlogId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; } = DateTime.Now;
    }
}
