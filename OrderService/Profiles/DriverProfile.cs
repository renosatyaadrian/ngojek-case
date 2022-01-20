using AutoMapper;
using OrderService.Dtos;
using OrderService.Models;

namespace OrderService.Profiles
{
    public class DriverProfile : Profile
    {
        public DriverProfile()
        {
            CreateMap<Driver, DriverDto>().ForMember(dest => dest.FullName, opt => opt.MapFrom(dri => (dri.FirstName + " " + dri.LastName))); ;
            CreateMap<Driver, SetPositionDto>();

            CreateMap<SetPositionDto, Driver>();
            CreateMap<DriverForCreateDto, Driver>();
            CreateMap<DriverForCreateDto, DriverDto>();
        }
    }
}
