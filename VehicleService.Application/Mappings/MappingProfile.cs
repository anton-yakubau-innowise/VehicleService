using AutoMapper;
using VehicleService.Application.Vehicles.Dtos;
using VehicleService.Domain.Entities;

namespace VehicleService.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Vehicle, VehicleDto>()
                .ForMember(dest => dest.BasePriceAmount, opt => opt.MapFrom(src => src.BasePrice.Amount))
                .ForMember(dest => dest.BasePriceCurrency, opt => opt.MapFrom(src => src.BasePrice.Currency));
        
        }
    }
}