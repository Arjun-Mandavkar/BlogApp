using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Repositories;
using BlogApp.Utilities.MappingUtils;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Services.UserServices.Implementations
{
    public class UserCrudService : IUserCrudService
    {
        private IMyUserStore _userStore;
        private IUserMapper _userMapper;
        private IXResultServiceMapper _resultMapper;
        private PasswordHasher<ApplicationUser> _hasher;
        public UserCrudService(IMyUserStore userStore,
                               IUserMapper userMapper,
                               IXResultServiceMapper identityServiceMapper,
                               PasswordHasher<ApplicationUser> hasher)
        {
            _userStore = userStore;
            _userMapper = userMapper;
            _resultMapper = identityServiceMapper;
            _hasher = hasher;
        }
        public async Task<UserInfoDto> CreateUser(RegisterUserDto dto)
        {
            string passwordHash = _hasher.HashPassword(new ApplicationUser(), dto.Password);
            IdentityResult result = await _userStore.CreateAsync(_userMapper.Map(dto, passwordHash), CancellationToken.None);
            if (!result.Succeeded)
                return null;
            return await FindByEmail(dto.Email);
        }

        public async Task<ServiceResult> DeleteUser(string userId)
        {
            ApplicationUser user = await _userStore.FindByIdAsync(userId, CancellationToken.None);
            if (user == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "User not found." });
            IdentityResult res = await _userStore.DeleteAsync(user, CancellationToken.None);
            return _resultMapper.Map(res);
        }

        public async Task<UserInfoDto> FindByEmail(string email)
        {
            ApplicationUser user = await _userStore.FindByNameAsync(email, CancellationToken.None);
            return user != null? _userMapper.Map(user): null;
        }

        public async Task<UserInfoDto> FindById(string userId)
        {
            ApplicationUser user = await _userStore.FindByIdAsync(userId, CancellationToken.None);
            return user != null ? _userMapper.Map(user) : null;
        }

        public async Task<ServiceResult> SoftDeleteUser(string email)
        {
            ApplicationUser user = await _userStore.FindByNameAsync(email, CancellationToken.None);

            //Chech user exists or not
            if (user == null)
                ServiceResult.Failed(new Message { Code = "Error", Description = "User not found." });

            IdentityResult result = await _userStore.SoftDeleteAsync(email);

            if (result.Succeeded)
                return ServiceResult.Success(new Message { Code = "Message", Description = "User deleted successfully." });

            return _resultMapper.Map(result);
        }

        public async Task<ServiceResult> UpdateUser(RegisterUserDto dto)
        {
            string passwordHash = _hasher.HashPassword(new ApplicationUser(), dto.Password);
            ApplicationUser detachedUser = _userMapper.Map(dto, passwordHash);

            IdentityResult result = await _userStore.UpdateAsync(detachedUser, CancellationToken.None);
            ServiceResult res = _resultMapper.Map(result);

            if (result.Succeeded)
            {
                res.Messages.Append(new Message { Code = "Message", Description = "User updates successfully." });
            }
            return res;
        }
    }
}
