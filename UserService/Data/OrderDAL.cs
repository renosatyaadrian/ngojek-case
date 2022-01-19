using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Dtos;
using UserService.Models;

namespace UserService.Data
{
    public class OrderDAL : IOrder
    {
        private readonly AppDbContext _dbContext;

        public OrderDAL(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> UpdateAcceptedOrder(UpdateAcceptedOrderDto acceptedOrderDto)
        {
            try
            {
                var order = await _dbContext.Orders.FirstOrDefaultAsync(order => order.Id == acceptedOrderDto.OrderId);
                if(order == null) throw new Exception($"Order id {acceptedOrderDto.OrderId} tidak di temukan");
                if(order.PickedUp == true) throw new Exception($"Order sudah di ambil");
                order.PickedUp = true;
                order.DriverId = acceptedOrderDto.DriverId;
                await _dbContext.SaveChangesAsync();
                return order;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Error : {ex.Message}");
            }
        }

        public async Task<Order> UpdateCompletedOrder(UpdateCompletedOrderDto completedOrderDto)
        {
            try
            {
                var order = await _dbContext.Orders.FirstOrDefaultAsync(order => order.Id == completedOrderDto.OrderId);
                if(order == null) throw new Exception($"Order id {completedOrderDto.OrderId} tidak di temukan");
                if(order.Completed == true) throw new Exception($"Order sudah selesai / dibayar");
                order.Completed = true;

                var user = await _dbContext.Customers.FirstOrDefaultAsync(cust => cust.Id == order.CustomerId);
                if(user.Balance<order.Price) throw new Exception($"Dana di saldo kurang, mohon top up terlebih dahulu"); 
                user.Balance-=order.Price;

                var driver = await _dbContext.Drivers.FirstOrDefaultAsync(driver => driver.Id == driver.Id);
                driver.Balance+=order.Price;
                
                await _dbContext.SaveChangesAsync();
                return order;

            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Error : {ex.Message}");
            }
        }
    }
}