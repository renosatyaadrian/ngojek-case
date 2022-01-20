using System;
using System.Collections.Generic;

#nullable disable

namespace OrderService.Models
{
    public partial class Driver
    {
        public Driver()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public double DriverLatitude { get; set; }
        public double DriverLongitude { get; set; }
        public double Balance { get; set; }
        public bool Blocked { get; set; }
        public string Username { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
