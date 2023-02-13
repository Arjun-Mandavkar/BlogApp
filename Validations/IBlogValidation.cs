using BlogApp.Models.Dtos;
using BlogApp.Models.Response;

namespace BlogApp.Validations
{
    public interface IBlogValidation
    {
        public Task<ValidationResult> ValidateBlog(BlogDto blog);

        public Task<ValidationResult> ValidateComment(BlogCommentDto comment);
    }
}
