using AdminService.Dtos;
using AdminService.Models;
using AutoMapper;

namespace AdminService.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>().ForMember(dest => dest.FullName, opt => opt.MapFrom(dri => $"{dri.FirstName} {dri.LastName}"));

            CreateMap<Customer, CustomerDto>();
        }
    }
}
