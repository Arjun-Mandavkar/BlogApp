using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Repositories;
using BlogApp.Validations;

namespace BlogApp.Services.UserServices.Implementations
{
    public class UserAuthService : IUserAuthService
    {
        private IUserValidation _userValidation;
        private IMyUserStore _userStore;
        public UserAuthService(IUserValidation userValidation, IMyUserStore userStore)
        {
            _userValidation = userValidation;
            _userStore = userStore;
        }

        public async Task<ServiceResult> VerifyPassword(UserInfoDto userDto, string password)
        {
            ApplicationUser user = await _userStore.FindByIdAsync(userDto.Id.ToString(), CancellationToken.None);
            if (user == null)
            {
                return ServiceResult.Failed(new Message { Code = "Error", Description = "User not found."});
            }
            else
            {
                if(await _userValidation.ValidatePassword(user, password))
                {
                    return ServiceResult.Success();
                }
                else
                {
                    return ServiceResult.Failed(new Message { Code = "Error", Description = "Something went wrong while validating password." });
                }
            }
        }
    }
}
