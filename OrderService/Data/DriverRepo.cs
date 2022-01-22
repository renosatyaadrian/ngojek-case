using Microsoft.AspNetCore.Http;
using OrderService.Dtos;
using OrderService.Helpers;
using OrderService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OrderService.Data
{
    public class DriverRepo : IDriverRepo
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
        public Driver ShowSaldo()
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            if (userName == null)
            {
                throw new Exception("Silahkan Login Terlebih Dahulu");
            }
            return _context.Drivers.FirstOrDefault(dri => dri.Username == userName);
        }

        //Order
        public IEnumerable<Order> GetAllOrders()
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var driver = _context.Drivers.FirstOrDefault(dri => dri.Username == userName);

            var order = _context.Orders.Where(o => o.PickedUp.Equals(false) && 
                o.Completed.Equals(false) &&
                o.DriverId == null).ToList();

            if (order != null)
            {
                return order;
            }
            else
            {
                throw new ArgumentNullException("Tidak ada orderan di sekitar anda");
            }
        }
        public IEnumerable<Order> GetHistoryOrder()
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var driver = _context.Drivers.FirstOrDefault(dri => dri.Username == userName);

            var order = _context.Orders.Where(o => o.DriverId == driver.Id && 
            o.PickedUp.Equals(true) && 
            o.Completed.Equals(true)).ToList();

            if (order != null)
            {
                return order;
            }
            else
            {
                throw new ArgumentNullException("Tidak ada orderan di sekitar anda");
            }
        }
        public void AcceptOrder(CustIdDto custIdDto)
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var driver = _context.Drivers.FirstOrDefault(dri => dri.Username == userName);
            var result = _context.Orders.FirstOrDefault(ord => ord.CustomerId == custIdDto.CustomerId && 
            ord.PickedUp.Equals(false) && 
            ord.Completed.Equals(false));

            if (result == null && result.Equals(true))
            {
                throw new Exception($"Order id dengan Customer Id {result.CustomerId} tidak di temukan");
            }

            result.DriverId = driver.Id;
            result.PickedUp = true;
            _context.SaveChanges();
        }

        public void FinishOrder(CustIdDto custIdDto)
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var driver = _context.Drivers.FirstOrDefault(dri => dri.Username == userName);
            var customer = _context.Customers.FirstOrDefault(cust => cust.Id == custIdDto.CustomerId);
            var result = _context.Orders.FirstOrDefault(ord => ord.CustomerId == custIdDto.CustomerId &&
            ord.PickedUp.Equals(true) && 
            ord.Completed.Equals(false));

            if (result == null && result.Completed.Equals(true))
            {
                throw new Exception($"Order id dengan Customer Id {result.CustomerId} tidak di temukan");
            }

            result.Completed = true;
            driver.Balance += result.Price;
            customer.Balance -= result.Price;
            _context.SaveChanges();


        }
    }
}
