using Microsoft.AspNetCore.Identity;

namespace BlogApp.Repositories
{
    public interface IBlogOwnersStore<TBlogOwner>
    {
        public Task<bool> IsOwner(int userId, int blogId);
        public Task<IdentityResult> AssignOwner(TBlogOwner blog);
        public Task<IdentityResult> RevokeOwner(int userId, int blogId);
        public Task<TBlogOwner> Get(int userId, int blogId);
        public Task<IEnumerable<int>> Get(int blogId);
        public Task<IdentityResult> Update(TBlogOwner blog);
        public Task<IdentityResult> SetIsOwnerExistsFalse(int userId);
    }
}
