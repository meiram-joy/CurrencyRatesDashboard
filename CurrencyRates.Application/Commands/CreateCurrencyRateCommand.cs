using CSharpFunctionalExtensions;
using CurrencyRates.Application.DTOs;
using MediatR;

namespace CurrencyRates.Application.Commands;

public record CreateCurrencyRateCommand(CurrencyRateDto currencyRate) : IRequest<Result>;