using BlogApp.Models.Dtos;
using BlogApp.Models.Response;
using Microsoft.IdentityModel.Tokens;

namespace BlogApp.Validations.Implementations
{
    public class BlogValidation : IBlogValidation
    {
        private IConfiguration _configuration;
        public BlogValidation(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ValidationResult> ValidateBlog(BlogDto blog)
        {
            //Check for character limits
            int titleLimit = Int32.Parse(_configuration.GetSection("Blog:TitleCharLimit").Value);
            int contentLimit = Int32.Parse(_configuration.GetSection("Blog:ContentCharLimit").Value);

            if (blog.Title.IsNullOrEmpty() || blog.Content.IsNullOrEmpty())
            {
                return ValidationResult.Failed(new Message { Code = "Error", Description = "Title or content not allowed to be empty." });
            }
            if (blog.Title.Length > titleLimit)
            {
                return ValidationResult.Failed(new Message { Code = "Error", Description = $"Title char limit is {titleLimit}." });
            }
            if (blog.Content.Length > contentLimit)
            {
                return ValidationResult.Failed(new Message { Code = "Error", Description = $"Content char limit is {contentLimit}." });
            }

            return ValidationResult.Success;
        }

        public async Task<ValidationResult> ValidateComment(BlogCommentDto comment)
        {
            //Check for character limits
            int charLimit = Int32.Parse(_configuration.GetSection("Blog:Comment:CharLimit").Value);

            if (comment.Text.IsNullOrEmpty())
            {
                return ValidationResult.Failed(new Message { Code = "Error", Description = "Comment text not allowed to be empty." });
            }
            if (comment.Text.Length > charLimit)
            {
                return ValidationResult.Failed(new Message { Code = "Error", Description = $"Comment text char limit is {charLimit}." });
            }

            return ValidationResult.Success;
        }
    }
}
