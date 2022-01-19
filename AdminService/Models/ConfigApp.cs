using System.ComponentModel.DataAnnotations;

namespace AdminService.Models
{
    public class ConfigApp
    {
        [Key]
        public int Id { get; set; }
        public double PricePerKM { get; set; }
    }
}
