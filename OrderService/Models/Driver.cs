﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace OrderService.Models
{
    public partial class Driver
    {
        public Driver()
        {
            Orders = new HashSet<Order>();
        }

        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public double DriverLatitude { get; set; }
        public double DriverLongitude { get; set; }
        public double Balance { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
