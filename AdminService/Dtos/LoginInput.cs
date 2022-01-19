﻿using System.ComponentModel.DataAnnotations;

namespace AdminService.Dtos
{
    public class LoginInput
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
