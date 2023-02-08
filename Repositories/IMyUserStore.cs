using BlogApp.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Repositories
{
    public interface IMyUserStore : IUserStore<ApplicationUser>
    {
        public Task<IdentityResult> SoftDeleteAsync(string email);
    }
}
