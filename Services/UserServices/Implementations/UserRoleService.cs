using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Repositories;
using BlogApp.Utilities.MappingUtils;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Services.UserServices.Implementations
{
    public class UserRoleService : IUserRoleService
    {
        private IMyUserStore _userStore;
        private IXResultServiceMapper _resultMapper;
        public UserRoleService(IMyUserStore userStore, IXResultServiceMapper resultMapper)
        {
            _userStore = userStore;
            _resultMapper = resultMapper;
        }

        public async Task<ServiceResult> AssignRole(int userId, RoleEnum role)
        {
            ApplicationUser user = await _userStore.FindByIdAsync(userId.ToString(), CancellationToken.None);
            if (user == null)
            {
                return ServiceResult.Failed(new Message
                {
                    Code = "Error",
                    Description = "Invalid user id."
                });
            }
            else
            {
                user.Role = role;
                IdentityResult result =  await _userStore.UpdateAsync(user, CancellationToken.None);
                return _resultMapper.Map(result);
            }
        }
    }
}
