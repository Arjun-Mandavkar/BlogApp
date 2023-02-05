﻿using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models.Dtos
{
    public class BlogRoleDto
    {
        [Required]
        public int BlogId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public BlogRoleEnum[] Roles { get; set; }
    }
}
