using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models
{
    public class Driver
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public double DriverLatitude { get; set; }
        public double DriverLongitude { get; set; }
        public double Balance { get; set; }
        public bool Blocked { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}