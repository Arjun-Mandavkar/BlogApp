using BlogApp.Models;
using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Services.BlogServices;
using BlogApp.Services.MappingServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Boolean = BlogApp.Models.Response.Boolean;

namespace BloggingApplication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BlogController : ControllerBase
    {
        private IBlogCrudService _blogCrudService;
        private IBlogMapper _blogMapper;
        private IXResultServiceMapper _resultMapper;
        private IResponseMapper _responseMapper;
        private IBlogLikeService _blogLikeService;
        private IBlogCommentService _blogCommentService;
        private IBlogOwnerService _blogOwnerService;
        private IBlogEditorService _blogEditorService;
        private IBlogRoleService _blogRoleService;
        public BlogController(IBlogCrudService blogCrudService,
                              IBlogMapper blogMapper,
                              IXResultServiceMapper resultMapper,
                              IResponseMapper responseMapper,
                              IBlogLikeService blogLikeService,
                              IBlogCommentService blogCommentService,
                              IBlogOwnerService blogOwnerService,
                              IBlogEditorService blogEditorService,
                              IBlogRoleService blogRoleService)
        {
            _blogCrudService = blogCrudService;
            _blogMapper = blogMapper;
            _resultMapper = resultMapper;
            _responseMapper = responseMapper;
            _blogLikeService = blogLikeService;
            _blogCommentService = blogCommentService;
            _blogOwnerService = blogOwnerService;
            _blogEditorService = blogEditorService;
            _blogRoleService = blogRoleService;
        }
        /*--------------------- CRUD --------------------*/
        [HttpGet]
        public async Task<IEnumerable<BlogDto>> GetAll()
        {
            IEnumerable<Blog> blogs = await _blogCrudService.GetAll();
            IEnumerable<BlogDto> dtos = new List<BlogDto>();
            foreach (var b in blogs)
            {
                dtos.Append(_blogMapper.Map(b));
            }
            return dtos;
        }

        [HttpGet("{blogId}")]
        public async Task<BlogDto> Get(int blogId)
        {
            Blog dto = await _blogCrudService.Get(blogId);
            return _blogMapper.Map(dto);
        }

        [HttpPost("Create")]
        public async Task<ApiResponse> Create(BlogDto blog)
        {
            ServiceResult result = await _blogCrudService.Create(blog);
            return _responseMapper.Map(result);
        }

        [HttpPut("Update")]
        public async Task<ApiResponse> Update(BlogDto blog)
        {
            ServiceResult result = await _blogCrudService.Update(blog);
            return _responseMapper.Map(result);

        }
        [HttpDelete("{blogId}")]
        public async Task<ApiResponse> Delete(int blogId)
        {
            ServiceResult result = await _blogCrudService.Delete(blogId);
            return _responseMapper.Map(result);
        }


        /*------------------------ Likes --------------------------*/
        [HttpGet("IsLiked/{blogId}")]
        public async Task<ActionResult<ApiResponse>> IsBlogLiked(int blogId)
        {
            Boolean res = new Boolean();
            res.Result = await _blogLikeService.IsLiked(blogId);
            return _responseMapper.Map(res);
        }

        [HttpGet("Like/{blogId}")]
        public async Task<ActionResult<ApiResponse>> LikeBlog(int blogId)
        {
            ServiceResult result = await _blogLikeService.LikeBlog(blogId);
            return _responseMapper.Map(result);

        }
        [HttpDelete("Like/{blogId}")]
        public async Task<ActionResult<ApiResponse>> DeleteLikeBlog(int blogId)
        {
            ServiceResult result = await _blogLikeService.DeleteLikeBlog(blogId);
            return _responseMapper.Map(result);
        }

        /*----------------------- Comments ------------------------*/
        [HttpPost("Comment")]
        public async Task<ActionResult<ApiResponse>> CommentOnBlog(BlogCommentDto dto)
        {
            ServiceResult result = await _blogCommentService.CommentOnBlog(dto);
            return _responseMapper.Map(result);
        }

        [HttpDelete("Comment/Delete")]
        public async Task<ActionResult<ApiResponse>> DeleteComment(BlogCommentDto dto)
        {
            ServiceResult result = await _blogCommentService.DeleteComment(dto);
            return _responseMapper.Map(result);
        }

        [HttpPut("Comment")]
        public async Task<ActionResult<ApiResponse>> UpdateComment(BlogCommentDto dto)
        {
            ServiceResult result = await _blogCommentService.EditComment(dto);
            return _responseMapper.Map(result);
        }

        [HttpGet("Comment/{blogId}")]
        public async Task<ActionResult<ApiResponse>> GetAllCommentsOfBlog(int blogId)
        {
            IEnumerable<BlogComment> comments = await _blogCommentService.GetAllCommentsOfBlog(blogId);

            IEnumerable <BlogCommentDto> commentsdto = new List<BlogCommentDto>();
            foreach (var comment in comments)
                commentsdto.Append(_blogMapper.Map(comment));

            return _responseMapper.Map(commentsdto);
        }

        /*---------------------- Assign Role ----------------------*/

        [HttpGet("Authors/{blogId}")]
        public async Task<ActionResult<ApiResponse>> GetAuthors(int blogId)
        {
            BlogAuthorsDto result = new BlogAuthorsDto();
            result.Editors = await _blogEditorService.GetEditors(blogId);
            result.Owners = await _blogOwnerService.GetOwners(blogId);

            return _responseMapper.Map(result);
        }

        [HttpPost("AssignRoles")]
        public async Task<ActionResult<ApiResponse>> AssignRoles(BlogRoleDto dto)
        {
            ServiceResult result = await _blogRoleService.AssignRoles(dto);
            return _responseMapper.Map(result);
        }

        [HttpPost("RevokeRoles")]
        public async Task<ActionResult<ApiResponse>> RevokeRoles(BlogRoleDto dto)
        {
            ServiceResult result = await _blogRoleService.RevokeRoles(dto);
            return _responseMapper.Map(result);
        }
    }
}