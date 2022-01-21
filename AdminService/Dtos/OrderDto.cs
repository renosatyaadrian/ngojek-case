namespace AdminService.Dtos
{
    public class OrderDto
    {
        public int DriverId { get; set; }
        public int CustomerId { get; set; }
        public double? Price { get; set; }
        public float? Distance { get; set; }
        public double UserLatitude { get; set; }
        public double UserLongitude { get; set; }
    }
}
