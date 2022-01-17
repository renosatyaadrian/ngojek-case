using AutoMapper;
using DriverService.Dtos;
using DriverService.Models;

namespace DriverService.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>();

            CreateMap<AcceptOrderDto, Order>();
            CreateMap<FinishOrderDto, Order>();
        }
    }
}
