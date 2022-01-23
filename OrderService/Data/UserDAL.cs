using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Data
{
    public class UserDAL : IUser
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserDAL(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<Order> GetOrderById(int id, int orderId)
        {
            try
            {
                var order = await _dbContext.Orders.FirstOrDefaultAsync(ord => ord.Id == orderId && ord.CustomerId == id);
                return order;
            }
            catch (System.Exception)
            {
                throw new Exception($"Order tidak ditemukan");
            }
        }

        public async Task<ICollection<Order>> GetOrdersHistory(int id)
        {
            return await _dbContext.Orders.Where(ord => ord.CustomerId == id).ToListAsync();
        }
    }
}