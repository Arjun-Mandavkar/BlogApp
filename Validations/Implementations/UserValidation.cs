using BlogApp.Models;
using BlogApp.Services.UserServices;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Validations.Implementations
{
    public class UserValidation : IUserValidation
    {
        private IUserCrudService _userCrudService;
        private PasswordHasher<ApplicationUser> _hasher;
        public UserValidation(IUserCrudService userCrudService, PasswordHasher<ApplicationUser> hasher)
        {
            _userCrudService = userCrudService;
            _hasher = hasher;
        }

        public async Task<bool> ValidateEmail(string email)
        {
            ApplicationUser user = await _userCrudService.FindByEmail(email);
            return user != null;
        }

        public async Task<bool> ValidatePassword(ApplicationUser user, string password)
        {
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
