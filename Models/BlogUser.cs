﻿using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class BlogUser
    {
        [Required]
        public int BlogId { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}