using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Services.BlogServices
{
    public interface IBlogCrudService
    {
        public Task<ServiceResult> Create(BlogDto blog);
        public Task<BlogDto> Get(int blogId);
        public Task<IEnumerable<BlogDto>> GetAll();
        public Task<ServiceResult> Delete(int blogId);
        public Task<ServiceResult> Update(BlogDto blog);
    }
}
