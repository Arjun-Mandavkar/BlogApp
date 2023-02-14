using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Models.ServiceObjects;

namespace BlogApp.Services.BlogServices
{
    public interface IBlogCrudService
    {
        public Task<ServiceResult> Create(BlogDto blog);
        public Task<BlogServiceObject> Get(int blogId);
        public Task<IEnumerable<BlogServiceObject>> GetAll();
        public Task<ServiceResult> Delete(int blogId);
        public Task<ServiceResult> Update(BlogDto blog);
    }
}
