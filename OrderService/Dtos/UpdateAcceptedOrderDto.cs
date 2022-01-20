using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Dtos
{
    public class UpdateAcceptedOrderDto
    {
        public bool Accepted { get; set; }
        public int DriverId { get; set; }
        public int OrderId { get; set; }
    }
}