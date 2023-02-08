using Microsoft.AspNetCore.Identity;

namespace BlogApp.Repositories
{
    public interface IUserRolesStore<TRole, TUser>
    {
        public Task<TRole> GetUserSingleRoleAsync(int userId, CancellationToken token);

        public Task<IdentityResult> AssignNewAsync(TRole role, TUser user, CancellationToken token);
    }
}
