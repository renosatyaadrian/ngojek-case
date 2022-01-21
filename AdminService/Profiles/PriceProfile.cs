using AdminService.Dtos;
using AdminService.Models;
using AutoMapper;

namespace AdminService.Profiles
{
    public class PriceProfile : Profile
    {
        public PriceProfile()
        {
            CreateMap<SetPriceCreateDto, ConfigApp>();
            CreateMap<ConfigApp, SetPriceDto>();
        }
    }
    
}
