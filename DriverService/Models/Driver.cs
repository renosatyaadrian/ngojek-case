using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DriverService.Models
{
    public class Driver
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int PhoneNumber { get; set; }
        public double? DriverLatitude { get; set; }
        public double? DriverLongitude { get; set; }
        [Required]
        public double Balance { get; set; }
        [Required]
        public bool Blocked { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
