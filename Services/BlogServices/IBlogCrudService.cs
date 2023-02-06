using BlogApp.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Services.BlogServices
{
    public interface IBlogCrudService
    {
        public Task<IdentityResult> Create(Blog blog);
        public Task<Blog> Get(int blogId);
        public Task<IEnumerable<Blog>> GetAll();
        public Task<IdentityResult> Delete(int blogId);
        public Task<IdentityResult> Update(Blog blog);
    }
}
