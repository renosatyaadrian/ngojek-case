using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Dtos
{
    public class UpdateCompletedOrderDto
    {
        public int OrderId { get; set; }
        public bool Completed { get; set; }
    }
}