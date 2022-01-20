using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Dtos;

namespace UserService.SyncDataServices.Http
{
    public interface IOrderDataClient
    {
        Task<IEnumerable<OrderDto>> GetUserOrdersHistory(int id);
        Task<OrderDto> GetUserOrderById(int id, int orderId);
    }
}