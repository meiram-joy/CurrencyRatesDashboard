using CSharpFunctionalExtensions;
using CurrencyRates.Application.Interfaces;
using MediatR;

namespace CurrencyRates.Application.Commands;

public class RefreshCurrencyRatesCommandHandler : IRequestHandler<RefreshCurrencyRatesCommand, Result<Unit>>
{
    private readonly ICurrencyRateService _service;

    public RefreshCurrencyRatesCommandHandler(ICurrencyRateService service)
    {
        _service = service;
    }
    
    public async Task<Result<Unit>> Handle(RefreshCurrencyRatesCommand request, CancellationToken cancellationToken)
    {
        await _service.RefreshRatesAsync(cancellationToken);
        return Unit.Value;
    }
}