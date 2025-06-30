using CSharpFunctionalExtensions;
using MediatR;

namespace CurrencyRates.Application.Commands;

public record RefreshCurrencyRatesCommand() : IRequest<Result<Unit>>;