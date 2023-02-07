using BlogApp.Models;

namespace BlogApp.Validations
{
    public interface IUserValidation
    {
        public Task<bool> ValidateEmail(string email);
        public Task<bool> ValidatePassword(ApplicationUser user, string password);
        public Task<bool> ValidateAdminUser(ApplicationUser user);
    }
}
