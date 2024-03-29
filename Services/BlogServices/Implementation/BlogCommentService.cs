﻿using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Models.ServiceObjects;
using BlogApp.Repositories;
using BlogApp.Utilities.JwtUtils;
using BlogApp.Utilities.MappingUtils;
using BlogApp.Validations;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Services.BlogServices.Implementation
{
    public class BlogCommentService : IBlogCommentService
    {
        private IBlogStore<Blog> _blogStore;
        private IBlogCommentsStore<BlogComment> _blogCommentStore;
        private IBlogValidation _blogValidation;
        private IXResultServiceMapper _resultMapper;
        private IBlogMapper _blogMapper;
        private IUserValidation _userValidation;
        private IBlogRoleValidation _blogRoleValidation;
        private IAuthUtils _authUtils;
        private IUserStore<ApplicationUser> _userStore;
        private IServiceObjectMapper _serviceObjectMapper;
        public BlogCommentService(IBlogStore<Blog> blogStore,
                                  IBlogCommentsStore<BlogComment> blogCommentStore,
                                  IBlogValidation blogValidation,
                                  IXResultServiceMapper resultMapper,
                                  IBlogMapper blogMapper,
                                  IUserValidation userValidation,
                                  IBlogRoleValidation blogRoleValidation,
                                  IAuthUtils authUtils,
                                  IUserStore<ApplicationUser> userStore,
                                  IServiceObjectMapper serviceObjectMapper)
        {
            _blogStore = blogStore;
            _blogCommentStore = blogCommentStore;
            _blogValidation = blogValidation;
            _resultMapper = resultMapper;
            _blogMapper = blogMapper;
            _userValidation = userValidation;
            _blogRoleValidation = blogRoleValidation;
            _authUtils = authUtils;
            _userStore = userStore;
            _serviceObjectMapper = serviceObjectMapper;
        }

        public async Task<ServiceResult> CommentOnBlog(BlogCommentDto comment)
        {
            if (comment.Id != 0)
                return ServiceResult.Failed(new Message { Code = "Message", Description = "Comment object should be transient [id should be null]." });

            ValidationResult res = await _blogValidation.ValidateComment(comment);
            if(!res.Succeeded)
                return _resultMapper.Map(res);

            string userId = await _authUtils.GetLoggedInUserId();
            ApplicationUser loggedInUser = await _userStore.FindByIdAsync(userId, CancellationToken.None);

            //Create entity of BlogComment
            BlogComment commentEntity = _blogMapper.Map(comment, loggedInUser);

            BlogComment detachedComment = await _blogCommentStore.CreateAsync(commentEntity);
            if (detachedComment == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "Comment insertion failed." });

            return ServiceResult.Success(new Message { Code = "Message", Description = "Comment inserted successfully." });
        }

        public async Task<ServiceResult> DeleteComment(int commentId)
        {
            //check for valid comment
            BlogComment detachedObject = await _blogCommentStore.GetAsync(commentId);
            if (detachedObject == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "Invalid comment Id." });

            string userId = await _authUtils.GetLoggedInUserId();
            ApplicationUser loggedInUser = await _userStore.FindByIdAsync(userId, CancellationToken.None);

            //Check for valid blog
            Blog blog = await _blogStore.GetByIdAsync(detachedObject.BlogId);
            if (blog == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "Invalid blog Id." });

            //Check for correct user
            bool isAdmin = await _userValidation.ValidateAdminUser(loggedInUser);
            bool isOwner = await _blogRoleValidation.ValidateOwner(loggedInUser, blog);
            bool isCommentOwner = detachedObject.UserId == loggedInUser.Id;

            if(isAdmin || isOwner || isCommentOwner)
            {
                IdentityResult result = await _blogCommentStore.DeleteAsync(detachedObject);
                if (!result.Succeeded)
                    return ServiceResult.Failed(new Message { Code = "Error", Description = "Comment deletion failed." });

                return ServiceResult.Success(new Message { Code = "Message", Description = "Comment deleted successfully." });
            }
            else
            {
                return ServiceResult.Failed(new Message { Code = "Error", Description = "You are not authorized to delete comment." });
            }
        }

        public async Task<ServiceResult> EditComment(BlogCommentDto comment)
        {
            if (comment.Id == 0)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "Comment object should be detached." });

            BlogComment detachedObject = await _blogCommentStore.GetAsync(comment.Id);

            string userId = await _authUtils.GetLoggedInUserId();
            ApplicationUser loggedInUser = await _userStore.FindByIdAsync(userId, CancellationToken.None);

            //Check for correct user
            if (loggedInUser.Id != detachedObject.UserId)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "Not authorized to edit the comment." });

            //Check for correct blog id
            if (detachedObject.BlogId != comment.BlogId)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "Invalid combination of blog and comment." });

            //Validate comment
            ValidationResult res = await _blogValidation.ValidateComment(comment);
            if (!res.Succeeded)
                return _resultMapper.Map(res);

            IdentityResult result = await _blogCommentStore.UpdateAsync(detachedObject);
            if (!result.Succeeded)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "Comment updation failed." });

            return ServiceResult.Success(new Message { Code = "Message", Description = "Comment updated successfully." });
        }

        public async Task<IEnumerable<BlogCommentServiceObject>> GetAllCommentsOfBlog(int blogId)
        {
            IEnumerable<BlogComment> comments = await _blogCommentStore.GetAllFromBlogAsync(blogId);
            IEnumerable<BlogCommentServiceObject> result = new List<BlogCommentServiceObject>();

            foreach (BlogComment comment in comments)
                result.Append(_serviceObjectMapper.Map(comment));

            return result;
        }
    }
}
