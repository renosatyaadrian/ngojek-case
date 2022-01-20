using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.Dtos;
using OrderService.Models;

namespace OrderService.Data
{
    public interface IOrder
    {
        Task<Order> UpdateAcceptedOrder(UpdateAcceptedOrderDto acceptedOrderDto);
        Task<Order> UpdateCompletedOrder(UpdateCompletedOrderDto completedOrderDto);
    }
}