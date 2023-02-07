using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;

namespace BlogApp.Services.BlogServices
{
    public interface IBlogOwnerStore : IBlogRoleService
    {
        public Task<bool> UpdateOwnerEntryForUserDeletion(int userId);
    }
}
