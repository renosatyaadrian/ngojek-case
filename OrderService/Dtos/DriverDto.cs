using System;

namespace OrderService.Dtos
{
    public class DriverDto
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public double? DriverLatitude { get; set; }
        public double? DriverLongitude { get; set; }
        public double Balance { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
