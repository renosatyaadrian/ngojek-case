using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Dtos;
using UserService.Models;

namespace UserService.Data
{
    public interface IOrder
    {
        Task<Order> UpdateAcceptedOrder(UpdateAcceptedOrderDto acceptedOrderDto);
        Task<Order> UpdateCompletedOrder(UpdateCompletedOrderDto completedOrderDto);
    }
}