using BlogApp.Models;
using BlogApp.Models.Dtos;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Validations.Implementations
{
    public class UserValidation : IUserValidation
    {
        private IUserStore<ApplicationUser> _userStore;
        private PasswordHasher<ApplicationUser> _hasher;
        public UserValidation(IUserStore<ApplicationUser> userStore, PasswordHasher<ApplicationUser> hasher)
        {
            _userStore = userStore;
            _hasher = hasher;
        }

        public async Task<bool> ValidateAdminUser(ApplicationUser user)
        {
            return user.Role.Equals(RoleEnum.ADMIN);
        }

        public async Task<bool> ValidateEmail(string email)
        {
            ApplicationUser user = await _userStore.FindByNameAsync(email, CancellationToken.None);
            return user != null;
        }

        public async Task<bool> ValidatePassword(ApplicationUser user, string password)
        {
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
