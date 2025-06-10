using CurrencyRates.Application.DTOs;
using MediatR;

namespace CurrencyRates.Application.Queries;

public record GetCurrencyRatesQuery() : IRequest<IReadOnlyList<CurrencyRateDto>>;