namespace BlogApp.Models.Dtos
{
    public class BlogRoleDto
    {
        public int BlogId { get; set; }
        public int UserId { get; set; }
        public BlogRoleEnum[] Roles { get; set; }
    }
}
