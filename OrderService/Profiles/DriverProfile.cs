using AutoMapper;
using OrderService.Dtos;
using OrderService.Helpers;
using OrderService.Models;

namespace OrderService.Profiles
{
    public class DriverProfile : Profile
    {
        public DriverProfile()
        {
            CreateMap<Driver, DriverDto>().ForMember(dest => dest.FullName, opt => opt.MapFrom(dri => (dri.FirstName + " " + dri.LastName))); ;
            CreateMap<Driver, SetPositionDto>();
            CreateMap<Driver, ReadSaldoDto>().ForMember(dest => dest.Balance, opt => opt.MapFrom(dri => MathHelper.ToRupiah(dri.Balance)));

            CreateMap<SetPositionDto, Driver>();
            CreateMap<DriverForCreateDto, Driver>();
            CreateMap<DriverForCreateDto, DriverDto>();
        }
    }
}
