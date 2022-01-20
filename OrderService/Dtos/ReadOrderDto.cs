namespace OrderService.Dtos
{
    public class ReadOrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public double? Price { get; set; }
        public float? Distance { get; set; }
        public double UserLatitude { get; set; }
        public double UserLongitude { get; set; }
    }
}
