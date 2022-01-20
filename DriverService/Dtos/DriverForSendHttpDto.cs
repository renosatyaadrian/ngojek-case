using System;

namespace DriverService.Dtos
{
    public class DriverForSendHttpDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public double? DriverLatitude { get; set; }
        public double? DriverLongitude { get; set; }
        public double Balance { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
