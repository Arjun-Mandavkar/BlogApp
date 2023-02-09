using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using BlogApp.Services.BlogServices;
using BlogApp.Utilities.MappingUtils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        private IResponseMapper _responseMapper;
        private IBlogLikeService _blogLikeService;
        private IBlogCommentService _blogCommentService;
        private IBlogOwnerService _blogOwnerService;
        private IBlogEditorService _blogEditorService;
        private IBlogRolesService _blogRolesService;
        public BlogController(IBlogCrudService blogCrudService,
                              IBlogMapper blogMapper,
                              IResponseMapper responseMapper,
                              IBlogLikeService blogLikeService,
                              IBlogCommentService blogCommentService,
                              IBlogOwnerService blogOwnerService,
                              IBlogEditorService blogEditorService,
                              IBlogRolesService blogRolesService)
        {
            _blogCrudService = blogCrudService;
            _blogMapper = blogMapper;
            _responseMapper = responseMapper;
            _blogLikeService = blogLikeService;
            _blogCommentService = blogCommentService;
            _blogOwnerService = blogOwnerService;
            _blogEditorService = blogEditorService;
            _blogRolesService = blogRolesService;
        }

        /*--------------------- CRUD --------------------*/
        [HttpGet]
        public async Task<ActionResult<List<BlogDto>>> GetAll()
        {
            IEnumerable<BlogDto> blogs = await _blogCrudService.GetAll();
            return Ok(blogs);
        }

        [HttpGet("{blogId}")]
        public async Task<ActionResult<ApiResponse<BlogDto>>> Get(int blogId)
        {
            BlogDto dto = await _blogCrudService.Get(blogId);
            return _responseMapper.Map(dto);
        }

        [HttpPost("Create")]
        public async Task<ApiResponse<List<Message>>> Create(BlogDto blog)
        {
            ServiceResult result = await _blogCrudService.Create(blog);
            return _responseMapper.Map(result);
        }

        [HttpPut("Update")]
        public async Task<ApiResponse<List<Message>>> Update(BlogDto blog)
        {
            ServiceResult result = await _blogCrudService.Update(blog);
            return _responseMapper.Map(result);

        }
        [HttpDelete("{blogId}")]
        public async Task<ApiResponse<List<Message>>> Delete(int blogId)
        {
            ServiceResult result = await _blogCrudService.Delete(blogId);
            return _responseMapper.Map(result);
        }


        /*------------------------ Likes --------------------------*/
        [HttpGet("IsLiked/{blogId}")]
        public async Task<ActionResult<ApiResponse<Boolean>>> IsBlogLiked(int blogId)
        {
            Boolean res = new Boolean();
            res.Result = await _blogLikeService.IsLiked(blogId);
            return Ok(_responseMapper.Map(res));
        }

        [HttpGet("Like/{blogId}")]
        public async Task<ActionResult<ApiResponse<List<Message>>>> LikeBlog(int blogId)
        {
            ServiceResult result = await _blogLikeService.LikeBlog(blogId);
            return Ok(_responseMapper.Map(result));
        }

        [HttpDelete("Like/{blogId}")]
        public async Task<ActionResult<ApiResponse<List<Message>>>> DeleteLikeBlog(int blogId)
        {
            ServiceResult result = await _blogLikeService.DeleteLikeBlog(blogId);
            return Ok(_responseMapper.Map(result));
        }

        /*----------------------- Comments ------------------------*/
        [HttpPost("Comment")]
        public async Task<ActionResult<ApiResponse<List<Message>>>> CommentOnBlog(BlogCommentDto dto)
        {
            ServiceResult result = await _blogCommentService.CommentOnBlog(dto);
            return Ok(_responseMapper.Map(result));
        }

        [HttpDelete("Comment/Delete")]
        public async Task<ActionResult<ApiResponse<List<Message>>>> DeleteComment(int commentId)
        {
            ServiceResult result = await _blogCommentService.DeleteComment(commentId);
            return Ok(_responseMapper.Map(result));
        }

        [HttpPut("Comment")]
        public async Task<ActionResult<ApiResponse<List<Message>>>> UpdateComment(BlogCommentDto dto)
        {
            ServiceResult result = await _blogCommentService.EditComment(dto);
            return Ok(_responseMapper.Map(result));
        }

        [HttpGet("Comment/{blogId}")]
        public async Task<ActionResult<ApiResponse<List<BlogCommentDto>>>> GetAllCommentsOfBlog(int blogId)
        {
            IEnumerable<BlogCommentDto> comments = await _blogCommentService.GetAllCommentsOfBlog(blogId);
            return Ok(_responseMapper.Map(comments.ToList()));
        }

        /*---------------------- Assign Role ----------------------*/

        [HttpGet("Authors/{blogId}")]
        public async Task<ActionResult<ApiResponse<BlogAuthorsDto>>> GetAuthors(int blogId)
        {
            BlogAuthorsDto result = new BlogAuthorsDto();
            result.Editors = await _blogEditorService.GetAll(blogId);
            result.Owners = await _blogOwnerService.GetAll(blogId);

            return Ok(_responseMapper.Map(result));
        }

        [HttpPost("AssignRoles")]
        public async Task<ActionResult<ApiResponse<List<Message>>>> AssignRoles(BlogRoleDto dto)
        {
            ServiceResult result = await _blogRolesService.AssignRoles(dto);
            return Ok(_responseMapper.Map(result));
        }

        [HttpPost("RevokeRoles")]
        public async Task<ActionResult<ApiResponse<List<Message>>>> RevokeRoles(BlogRoleDto dto)
        {
            ServiceResult result = await _blogRolesService.RevokeRoles(dto);
            return Ok(_responseMapper.Map(result));
        }
    }
}