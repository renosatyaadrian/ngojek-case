using AdminService.Dtos;
using AdminService.Helper;
using AdminService.Models;
using AutoMapper;

namespace AdminService.Profiles
{
    public class DriverProfile : Profile
    {
        public DriverProfile()
        {
            CreateMap<Driver, DriverDto>().ForMember(dest => dest.FullName, opt => opt.MapFrom(dri => $"{dri.FirstName} {dri.LastName}"));
            CreateMap<Driver, ReadSaldoDto>().ForMember(dest => dest.Balance, opt => opt.MapFrom(dri => MathHelper.ToRupiah(dri.Balance)));
        }
    }
}
