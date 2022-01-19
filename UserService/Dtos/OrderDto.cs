using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public double Price { get; set; }
        public float Distance { get; set; }
        public double UserLatitude { get; set; }
        public double UserLongitude { get; set; }
    }
}