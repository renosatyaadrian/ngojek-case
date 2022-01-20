using System;
using System.Collections.Generic;

#nullable disable

namespace OrderService.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public double Balance { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Blocked { get; set; }
        public string Username { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
