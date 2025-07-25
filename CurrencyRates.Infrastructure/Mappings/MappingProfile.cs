﻿using AutoMapper;
using CurrencyRates.Application.DTOs;
using CurrencyRates.Domain.Currency.Aggregates;
using CurrencyRates.Domain.Currency.Entities;

namespace CurrencyRates.Infrastructure.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CurrencyRate, CurrencyRateDto>()
            .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode));
        CreateMap<CurrencyRateAggregate, CurrencyRateDto>()
            .ForMember(dest => dest.CurrencyName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.RetrievedAt));
        CreateMap<CurrencyRateDto, CurrencyRateAggregate>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CurrencyName))
            .ForMember(dest => dest.RetrievedAt, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode));
    }
}