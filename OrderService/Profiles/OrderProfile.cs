using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using OrderService.Dtos;
using OrderService.Models;

namespace OrderService.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();
            CreateMap<Order, ReadOrderDto>();

            CreateMap<ReadOrderDto, Order>();
            CreateMap<Order, OrderFeeDto>();
            CreateMap<OrderFeeDto, Order>();
        }
    }
}