﻿using BlogApp.Models;
using BlogApp.Models.Dtos;

namespace BlogApp.Services.MappingServices
{
    public interface IBlogMapper : IMapper<BlogDto, Blog>,
                                   IMapper<Blog, BlogDto>,
                                   IMapper<BlogComment, BlogCommentDto>,
                                   IMapper<BlogUserObject, BlogOwner>
    {
        public BlogComment Map(BlogCommentDto dto, ApplicationUser user);
    }
}
