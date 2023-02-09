﻿using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Repositories;
using BlogApp.Services.UserServices;

namespace BlogApp.Services.BlogServices.Implementation
{
    public class BlogRolesService : IBlogRolesService
    {
        private IBlogStore<Blog> _blogStore;
        private IMyUserStore _userStore;
        private IBlogEditorService _blogEditorService;
        private IBlogOwnerService _blogOwnerService;
        public BlogRolesService(IBlogStore<Blog> blogStore,
                                IMyUserStore userStore,
                                IBlogEditorService blogEditorService,
                                IBlogOwnerService blogOwnerService)
        {
            _blogStore = blogStore;
            _userStore = userStore;
            _blogEditorService = blogEditorService;
            _blogOwnerService = blogOwnerService;
        }

        public async Task<ServiceResult> AssignRoles(BlogRoleDto dto)
        {
            Blog blog = await _blogStore.GetByIdAsync(dto.BlogId);
            if (blog == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "Blog not found." });

            ApplicationUser user = await _userStore.FindByIdAsync(dto.UserId.ToString(), CancellationToken.None);
            if (user == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "User not found." });

            if (dto.Roles.Contains(BlogRoleEnum.OWNER))
                return await _blogOwnerService.Assign(blog.Id, user.Id);
            else if (dto.Roles.Contains(BlogRoleEnum.EDITOR))           //This else is requirement
                return await _blogEditorService.Assign(blog.Id, user.Id);

            return ServiceResult.Success(new Message { Code = "Message", Description = "Roles assigned successfully."});
        }

        public async Task<ServiceResult> RevokeRoles(BlogRoleDto dto)
        {
            Blog blog = await _blogStore.GetByIdAsync(dto.BlogId);
            if (blog == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "Blog not found." });

            ApplicationUser user = await _userStore.FindByIdAsync(dto.UserId.ToString(), CancellationToken.None);
            if (user == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "User not found." });

            if (dto.Roles.Contains(BlogRoleEnum.OWNER))
                return await _blogOwnerService.Revoke(blog.Id, user.Id);
            if (dto.Roles.Contains(BlogRoleEnum.EDITOR))
                return await _blogEditorService.Revoke(blog.Id, user.Id);

            return ServiceResult.Success(new Message { Code = "Message", Description = "Roles revoked successfully." });
        }
    }
}
