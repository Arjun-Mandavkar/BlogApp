using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Services.MappingServices;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Services.UserServices.Implementation
{
    public class UserCrudService : IUserCrudService
    {
        private IUserStore<ApplicationUser> _userStore;
        private IUserMapper _userMapper;
        private IIdentityServiceMapper _identityServiceMapper;
        public UserCrudService(IUserStore<ApplicationUser> userStore, IUserMapper userMapper, IIdentityServiceMapper identityServiceMapper)
        {
            _userStore = userStore;
            _userMapper = userMapper;
            _identityServiceMapper = identityServiceMapper;
        }
        public async Task<ApplicationUser> CreateUser(RegisterUserDto dto)
        {
            IdentityResult result = await _userStore.CreateAsync(_userMapper.Map(dto), CancellationToken.None);
            if (!result.Succeeded)
                return null;
            return await FindByEmail(dto.Email);
        }

        public async Task<ServiceResult> DeleteUser(ApplicationUser user)
        {
            IdentityResult res = await _userStore.DeleteAsync(user, CancellationToken.None);
            return _identityServiceMapper.Map(res);
        }

        public async Task<ApplicationUser> FindByEmail(string email)
        {
            return await _userStore.FindByNameAsync(email, CancellationToken.None);
        }

        public async Task<ApplicationUser> FindById(string userId)
        {
            return await _userStore.FindByIdAsync(userId, CancellationToken.None);
        }

        public async Task<ServiceResult> UpdateUser(ApplicationUser detachedUser)
        {
            IdentityResult result = await _userStore.UpdateAsync(detachedUser, CancellationToken.None);
            return _identityServiceMapper.Map(result);
        }
    }
}
