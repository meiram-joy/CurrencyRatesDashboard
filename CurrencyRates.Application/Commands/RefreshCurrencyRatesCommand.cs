using MediatR;

namespace CurrencyRates.Application.Commands;

public record RefreshCurrencyRatesCommand() : IRequest, IRequest<Unit>;