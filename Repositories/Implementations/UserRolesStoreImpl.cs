using BlogApp.DbConnection;
using BlogApp.Models;
using Dapper;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Repositories.Implementations
{
    public class UserRolesStoreImpl : IUserRolesStore<IdentityRole, ApplicationUser>
    {
        private IDbConnectionFactory _connectionFactory { get; }
        public UserRolesStoreImpl(IDbConnectionFactory dbConnectionFactory)
        {
            _connectionFactory = dbConnectionFactory;
        }
        public async Task<IdentityRole> GetUserSingleRoleAsync(int userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string query1 = "SELECT [RoleId] FROM [UserRoles] WHERE UserId = @UserId";
            string query2 = "SELECT [Id],[Name],[NormalizedName] FROM [Roles] WHERE Id = @RoleId;";
            IdentityRole role = null;
            using (var connection = _connectionFactory.GetDefaultConnection())
            {
                await connection.OpenAsync(cancellationToken);
                int roleId = await connection.QuerySingleOrDefaultAsync<int>(query1, new { UserId = userId });
                role = await connection.QuerySingleOrDefaultAsync<IdentityRole>(query2, new { RoleId = roleId });
            }
            return role;
        }

        public async Task<IdentityResult> AssignNewAsync(IdentityRole role, ApplicationUser user, CancellationToken cancellationToken)
        {
            string query = $@"INSERT INTO [UserRoles] 
                               ([UserId],[RoleId]) 
                               VALUES (@{nameof(UserRole.UserId)},@{nameof(UserRole.RoleId)});";
            using (var connection = _connectionFactory.GetDefaultConnection())
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync(query,
                                              new UserRole { UserId = user.Id, RoleId = Int32.Parse(role.Id) });
            }
            return IdentityResult.Success;
        }
    }
}
