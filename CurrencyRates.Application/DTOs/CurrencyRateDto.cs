using CSharpFunctionalExtensions;
using MediatR;

namespace CurrencyRates.Application.DTOs;

public class CurrencyRateDto : IRequest<Result>
{
    public string CurrencyCode { get; set; } 
    public string CurrencyName { get; set; } 
    public double Rate { get; set; }
    public DateTime Date { get; set; }
}