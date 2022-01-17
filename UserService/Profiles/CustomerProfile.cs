using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UserService.Dtos;
using UserService.Models;
using UserService.Helper;

namespace UserService.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(cust => (cust.FirstName + " " + cust.LastName)))
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(cust => MathHelper.ToRupiah(cust.Balance)));
            CreateMap<Customer, CustomerBalanceDto>()
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(cust => MathHelper.ToRupiah(cust.Balance)));
            CreateMap<CustomerDto, Customer>();
        }
    }
}