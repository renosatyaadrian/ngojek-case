using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.Models;

namespace OrderService.Data
{
    public interface IUser
    {
        Task<Order> GetOrderById(int id, int orderId);
        Task<ICollection<Order>> GetOrdersHistory(int id);
    }
}