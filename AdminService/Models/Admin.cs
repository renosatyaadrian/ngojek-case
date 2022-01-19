using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DriverService.Models
{
    public class Admin
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required] 
        public string Username { get; set; }
    }
}
