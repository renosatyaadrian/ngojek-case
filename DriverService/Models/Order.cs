using System.ComponentModel.DataAnnotations;

namespace DriverService.Models
{
    public class Order
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }
        public int? DriverId { get; set; }
        public double? Price { get; set; }
        public float? Distance { get; set; }
        public bool PickedUp { get; set; }
        public bool Completed { get; set; }
        public double UserLatitude { get; set; }
        public double UserLongitude { get; set; }
        public Driver Driver { get; set; }
    }
}
