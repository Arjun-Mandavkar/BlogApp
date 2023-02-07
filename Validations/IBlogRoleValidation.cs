using BlogApp.Models;

namespace BlogApp.Validations
{
    public interface IBlogRoleValidation
    {
        public Task<bool> ValidateOwner(ApplicationUser user, Blog blog);
        public Task<bool> ValidateEditor(ApplicationUser user, Blog blog);
    }
}
