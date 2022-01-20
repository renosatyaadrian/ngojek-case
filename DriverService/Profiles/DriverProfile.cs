using AutoMapper;
using DriverService.Dtos;
using DriverService.Helper;
using DriverService.Models;

namespace DriverService.Profiles
{
    public class DriverProfile : Profile
    {
        public DriverProfile()
        {
            CreateMap<Driver, DriverDto>().ForMember(dest => dest.FullName, opt => opt.MapFrom(dri => (dri.FirstName + " " + dri.LastName))); ;
            CreateMap<Driver, ReadSaldoDto>().ForMember(dest => dest.Balance, opt => opt.MapFrom(dri => MathHelper.ToRupiah(dri.Balance))); ;
            CreateMap<Driver, SetPositionDto>();
            CreateMap<Driver, DriverForSendHttpDto>();

            CreateMap<DriverForSendHttpDto, Driver>();

            CreateMap<SetPositionDto, Driver>();

            CreateMap<DriverForCreateDto, Driver>();
            CreateMap<DriverForCreateDto, DriverDto>();
            CreateMap<DriverForCreateDto, DriverForSendHttpDto>();

            CreateMap<DriverForSendHttpDto, DriverForCreateDto>();
        }
    }
}
