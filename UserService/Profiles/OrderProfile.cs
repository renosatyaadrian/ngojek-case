using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UserService.Dtos;
using UserService.Models;

namespace UserService.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();
            
            CreateMap<Order, OrderFeeDto>();
            CreateMap<OrderFeeDto, Order>();
        }
    }
}