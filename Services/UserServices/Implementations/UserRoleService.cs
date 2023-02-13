﻿using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Utilities.MappingUtils;

namespace BlogApp.Services.UserServices.Implementations
{
    public class UserRoleService : IUserRoleService
    {
        private IUserCrudService _userCrudService;
        private IUserMapper _userMapper;
        public UserRoleService(IUserCrudService userCrudService)
        {
            _userCrudService = userCrudService;
        }

        public async Task<ServiceResult> AssignRole(int userId, RoleEnum role)
        {
            ApplicationUser user = await _userCrudService.FindById(userId.ToString());
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
                return await _userCrudService.UpdateUser(_userMapper.MapExt(user));
            }
        }
    }
}
