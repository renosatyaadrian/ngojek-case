using Microsoft.AspNetCore.Http;
using OrderService.Helpers;
using OrderService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OrderService.Data
{
    public class DriverRepo
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DriverRepo(AppDbContext context,
                          IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        //Profile
        public void SetPosition(Driver obj)
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var result = _context.Drivers.FirstOrDefault(dri => dri.Username == userName);

            result.DriverLongitude = obj.DriverLongitude;
            result.DriverLatitude = obj.DriverLatitude;
            _context.SaveChanges();
        }
        public void CreateDriver(Driver driver)
        {
            if (driver == null)
            {
                throw new ArgumentNullException(nameof(driver));
            }
            _context.Drivers.Add(driver);
        }
        //Order
        public IEnumerable<Order> GetAllOrders()
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var driver = _context.Drivers.FirstOrDefault(dri => dri.Username == userName);
            var order = _context.Orders.FirstOrDefault();

            if (MathHelper.getDistanceFromLatLonInKm((double)driver.DriverLatitude, (double)driver.DriverLongitude, order.UserLatitude, order.UserLongitude) <= 5 && order.PickedUp.Equals(false) && order.Completed.Equals(false))
            {
                return _context.Orders.ToList();
            }
            else
            {
                throw new Exception("Tidak ada orderan di sekitar anda");
            }
        }
        public Order GetHistoryOrder()
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var driver = _context.Drivers.FirstOrDefault(dri => dri.Username == userName);
            return _context.Orders.FirstOrDefault(ord => ord.DriverId == driver.Id);
        }
        public void AcceptOrder(int custId)
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var driver = _context.Drivers.FirstOrDefault(dri => dri.Username == userName);
            var result = _context.Orders.FirstOrDefault(ord => ord.CustomerId == custId);

            if (result == null)
            {
                throw new Exception($"Order id {result.Id} tidak di temukan");
            }

            result.DriverId = driver.Id;
            result.PickedUp = true;
            _context.SaveChanges();
        }

        public void FinishOrder(int custId)
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var driver = _context.Drivers.FirstOrDefault(dri => dri.Username == userName);
            var result = _context.Orders.FirstOrDefault(ord => ord.CustomerId == custId);

            if (result == null)
            {
                throw new Exception($"Order id {result.Id} tidak di temukan");
            }

            result.Completed = true;
            driver.Balance += (double)result.Price;
            _context.SaveChanges();
        }
    }
}
