﻿using BlogApp.DbConnection;
using BlogApp.Models;
using BlogApp.Models.Dtos;
using Dapper;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Repositories.Implementations
{
    public class BlogOwnersStoreImpl : IBlogOwnersStore<BlogOwner>
    {
        private IDbConnectionFactory _connectionFactory { get; }
        public BlogOwnersStoreImpl(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<bool> IsOwner(int userId, int blogId)
        {
            string query = $@"SELECT [BlogId],[UserId] FROM [BlogOwners]
                              WHERE UserId = @UserId 
                              AND BlogId = @BlogId";

            BlogOwner entry = null;
            using(var connection = _connectionFactory.GetDefaultConnection())
            {
                await connection.OpenAsync();
                entry = await connection.QuerySingleOrDefaultAsync<BlogOwner>(query, new { BlogId = blogId, UserId = userId });
            }
            return (entry == null) ? false : true;
        }
        public async Task<IdentityResult> AssignOwner(BlogOwner blog)
        {
            string query = $@"INSERT INTO [BlogOwners]
                              ([UserId],[BlogId],[OwnerName],[IsOwnerExists])
                              VALUES(@{nameof(BlogOwner.UserId)},@{nameof(BlogOwner.BlogId)},@{nameof(BlogOwner.OwnerName)},@{nameof(BlogOwner.IsOwnerExists)})";

            int? rowsAffected = null;

            using (var connection = _connectionFactory.GetDefaultConnection())
            {
                await connection.OpenAsync();
                rowsAffected = await connection.ExecuteAsync(query, blog);
            }
            if (rowsAffected == 1)
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> RevokeOwner(int userId, int blogId)
        {
            string query = $@"DELETE FROM [BlogOwners]
                              WHERE UserId = @UserId 
                              AND BlogId = @BlogId"; 

            int? rowsAffected = null;
            using (var connection = _connectionFactory.GetDefaultConnection())
            {
                await connection.OpenAsync();
                rowsAffected = await connection.ExecuteAsync(query, new { BlogId = blogId, UserId = userId });
            }
            if (rowsAffected == 1)
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<BlogOwner> Get(int userId, int blogId)
        {
            string query = $@"SELECT [BlogId],[UserId], [OwnerName],[IsOwnerExists]
                              FROM [BlogOwners]
                              WHERE UserId = @UserId 
                              AND BlogId = @BlogId";

            BlogOwner detachedObject = null;
            using (var connection = _connectionFactory.GetDefaultConnection())
            {
                await connection.OpenAsync();
                detachedObject = await connection.QuerySingleOrDefaultAsync<BlogOwner>(query, new {BlogId=blogId,UserId=userId});
            }
            return detachedObject;
        }
        public async Task<IEnumerable<int>> Get(int blogId)
        {
            string query = $@"SELECT [UserId]
                              FROM [BlogOwners]
                              WHERE BlogId = @BlogId
                              AND UserId != 0";
            IEnumerable<int> result = new List<int>();
            using (var connection = _connectionFactory.GetDefaultConnection())
            {
                await connection.OpenAsync();
                result = await connection.QueryAsync<int>(query, new { BlogId = blogId });
            }
            return result;
        }

        public async Task<IdentityResult> Update(BlogOwner blog)
        {
            string query = $@"UPDATE [BlogOwners]
                              SET [OwnerName]=@{nameof(BlogOwner.OwnerName)},[IsOwnerExists]=@{nameof(BlogOwner.IsOwnerExists)}
                              WHERE [UserId]=@{nameof(BlogOwner.UserId)} AND [BlogId]=@{nameof(BlogOwner.BlogId)};";
            
            int? rowsAffected = null;
            using (var connection = _connectionFactory.GetDefaultConnection())
            {
                await connection.OpenAsync();
                rowsAffected = await connection.ExecuteAsync(query, blog);
            }
            if (rowsAffected == 1)
                return IdentityResult.Success;
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> SetIsOwnerExistsFalse(int userId)
        {
            string query = $@"UPDATE [BlogOwners]
                              SET [IsOwnerExists]=@{nameof(BlogOwner.IsOwnerExists)},[UserId]=0
                              WHERE [UserId]=@{nameof(BlogOwner.UserId)};";

            using (var connection = _connectionFactory.GetDefaultConnection())
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(query, new BlogOwner { UserId = userId, IsOwnerExists = false });
            }
            return IdentityResult.Success;
        }
    }
}
