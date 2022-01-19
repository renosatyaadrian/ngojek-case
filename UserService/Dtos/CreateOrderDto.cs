using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dtos
{
    public class CreateOrderDto
    {
        public double UserTargetLatitude { get; set; }
        public double UserTargetLongitude { get; set; }
        public double UserLatitude { get; set; }
        public double UserLongitude { get; set; }
    }
}