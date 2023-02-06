using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;

namespace BlogApp.Services.BlogServices
{
    public interface IBlogEditorService
    {
        public Task<IEnumerable<UserInfoDto>> GetEditors(int blogId);
        public Task<ServiceResult> AssignEditor(Blog blog, ApplicationUser user);
        public Task<ServiceResult> RevokeEditor(Blog blog, ApplicationUser user);
    }
}
