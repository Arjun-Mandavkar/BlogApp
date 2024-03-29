﻿using Microsoft.AspNetCore.Identity;

namespace BlogApp.Repositories
{
    public interface IBlogCommentsStore<TComment>
    {
        public Task<TComment> GetAsync(int id);

        public Task<TComment> CreateAsync(TComment comment);

        public Task<IdentityResult> UpdateAsync(TComment comment);

        public Task<IdentityResult> DeleteAsync(TComment comment);

        public Task<IEnumerable<TComment>> GetAllFromBlogAsync(int blogId);

        public Task<IEnumerable<TComment>> GetAllFromUserAsync(int userId);
    }
}
