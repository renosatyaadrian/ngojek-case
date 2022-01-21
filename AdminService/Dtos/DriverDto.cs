namespace AdminService.Dtos
{
    public class DriverDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public double Balance { get; set; }
        public bool Blocked { get; set; }
    }
}
