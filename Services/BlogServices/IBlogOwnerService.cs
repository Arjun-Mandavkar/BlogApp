using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;

namespace BlogApp.Services.BlogServices
{
    public interface IBlogOwnerService
    {
        public Task<IEnumerable<UserInfoDto>> GetOwners(int blogId);
        public Task<ServiceResult> AssignOwner(Blog blog, ApplicationUser user);
        public Task<ServiceResult> RevokeOwner(Blog blog, ApplicationUser user);
        public Task<bool> UpdateOwnerEntryForUserDeletion(ApplicationUser user);
    }
}
