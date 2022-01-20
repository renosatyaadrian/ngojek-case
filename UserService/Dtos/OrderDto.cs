using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace UserService.Dtos
{
    public class OrderDto
    {
        public int CustomerId { get; set; }
        public int? DriverId { get; set; }
        public double Price { get; set; }
        public float Distance { get; set; }
        public bool? Completed { get; set; }
        public bool PickedUp { get; set; }
        public double UserLatitude { get; set; }
        public double UserLongitude { get; set; }

        public static implicit operator OrderDto(HttpResponseMessage v)
        {
            throw new NotImplementedException();
        }
    }
}