using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Repositories;
using BlogApp.Services.UserServices;
using BlogApp.Utilities.JwtUtils;
using BlogApp.Utilities.MappingUtils;
using BlogApp.Validations;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Services.BlogServices.Implementation
{
    public class BlogCrudService : IBlogCrudService
    {
        private IBlogStore<Blog> _blogStore;
        private IBlogValidation _blogValidation;
        private IBlogRoleValidation _blogRoleValidation;
        private IUserValidation _userValidation;
        private IBlogMapper _blogMapper;
        private IXResultServiceMapper _resultMapper;
        private IAuthUtils _authUtils;
        private IUserStore<ApplicationUser> _userStore;
        public BlogCrudService(IBlogStore<Blog> blogStore,
                               IBlogValidation blogValidation,
                               IBlogRoleValidation blogRoleValidation,
                               IUserValidation userValidation,
                               IBlogMapper blogMapper,
                               IXResultServiceMapper resultMapper,
                               IAuthUtils authUtils,
                               IUserStore<ApplicationUser> userStore)
        {
            _blogStore = blogStore;
            _blogValidation = blogValidation;
            _blogRoleValidation = blogRoleValidation;
            _userValidation = userValidation;
            _blogMapper = blogMapper;
            _resultMapper = resultMapper;
            _authUtils = authUtils;
            _userStore = userStore;
        }

        public async Task<ServiceResult> Create(BlogDto blog)
        {
            //Validate blog
            ValidationResult res = await _blogValidation.ValidateBlog(blog);
            if (res != ValidationResult.Success)
                return _resultMapper.Map(res);

            //Map dto to entity
            Blog entity = _blogMapper.Map(blog);

            //Create blog from blog store
            Blog detachedBlog = await _blogStore.CreateAsync(entity);
            if (detachedBlog.Id == 0)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "Blog creation failed." });
            return ServiceResult.Success(new Message { Code = "Message", Description = "Blog created successfully." });
        }

        public async Task<ServiceResult> Delete(int blogId)
        {
            Blog blog = await _blogStore.GetByIdAsync(blogId);
            if (blog == null)
                return ServiceResult.Failed(new Message { Code = "Error", Description = "Invalid blog Id OR blog already deleted." });

            //Fetch logged in user
            string userId = await _authUtils.GetLoggedInUserId();
            ApplicationUser loggedInUser = await _userStore.FindByIdAsync(userId, CancellationToken.None);

            //Check wheather user is one of the owners or admin
            bool isOwner = await _blogRoleValidation.ValidateOwner(loggedInUser, blog);
            bool isAdmin = await _userValidation.ValidateAdminUser(loggedInUser);

            //Delete blog from blog store
            if (isAdmin || isOwner)
            {
                IdentityResult result = await _blogStore.DeleteAsync(blog.Id);
                if (!result.Succeeded)
                    return ServiceResult.Failed(new Message { Code = "Error", Description = "Blog deletion failed." });
            }
            else
                return ServiceResult.Failed(new Message { Code = "Unauthorized", Description = "You are not allowed to delete this blog." });

            return ServiceResult.Success(new Message { Code = "Message", Description = "Blog deleted successfully." });
        }

        public async Task<BlogDto> Get(int blogId)
        {
            Blog blog = await _blogStore.GetByIdAsync(blogId);
            BlogDto dto = _blogMapper.Map(blog);
            return dto;
        }

        public async Task<IEnumerable<BlogDto>> GetAll()
        {
            IEnumerable<Blog> blogs = await _blogStore.GetAllAsync();
            IEnumerable<BlogDto> dtos = new List<BlogDto>();
            foreach (Blog blog in blogs)
            {
                dtos.Append(_blogMapper.Map(blog));
            }
            return dtos;
        }

        public async Task<ServiceResult> Update(BlogDto blog)
        {
            //Validate blog
            ValidationResult res = await _blogValidation.ValidateBlog(blog);
            if (res != ValidationResult.Success)
                return _resultMapper.Map(res);

            //Map dto to entity
            Blog blogEntity = _blogMapper.Map(blog);

            //Fetch logged in user
            string userId = await _authUtils.GetLoggedInUserId();
            ApplicationUser loggedInUser = await _userStore.FindByIdAsync(userId, CancellationToken.None);

            //Check wheather user is one of the owners or editors or admin
            bool isOwner = await _blogRoleValidation.ValidateOwner(loggedInUser, blogEntity);

            bool isEditor = await _blogRoleValidation.ValidateEditor(loggedInUser, blogEntity);

            bool isAdmin = await _userValidation.ValidateAdminUser(loggedInUser);

            if (isAdmin || isOwner || isEditor)
            {
                IdentityResult result = await _blogStore.UpdateAsync(blogEntity);
                if (!result.Succeeded)
                    return ServiceResult.Failed(new Message { Code = "Error", Description = "Blog updation failed." });
            }
            else
                return ServiceResult.Failed(new Message { Code = "Unauthorized", Description = "You are not allowed to edit this blog." });

            return ServiceResult.Success(new Message { Code = "Message", Description = "Blog updated successfully." });
        }
    }
}
