using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models
{
    public class ConfigApp
    {
        [Key]
        public int Id { get; set; }
        public double PricePerKM { get; set; }
    }
}