﻿namespace DriverService.Dtos
{
    public class OrderDto
    {
        public int? Id { get; set; }
        public int CustomerId { get; set; }
        public int? DriverId { get; set; }
        public double Price { get; set; }
        public float Distance { get; set; }
        public bool? Completed { get; set; }
        public bool PickedUp { get; set; }
        public double UserLatitude { get; set; }
        public double UserLongitude { get; set; }
    }
}
