﻿using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models.Dtos
{
    public class BlogCommentDto
    {
        public int Id { get; set; }
        [Required]
        public int BlogId { get; set; }
        [Required]
        public string Text { get; set; } = string.Empty;
    }
}
