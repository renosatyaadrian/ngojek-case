using AdminService.Dtos;
using AdminService.Models;
using AutoMapper;

namespace AdminService.Profiles
{
    public class OrderProfile :Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>();
        }
    }
}
