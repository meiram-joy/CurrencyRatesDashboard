using AutoMapper;
using CurrencyRates.Application.DTOs;
using CurrencyRates.Domain.Currency.Entities;

namespace CurrencyRates.Infrastructure.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CurrencyRate, CurrencyRateDto>()
            .ForMember(dest => dest.CurrencyCode, opt =>
            opt.MapFrom(src => src.CurrencyCode));
    }
}