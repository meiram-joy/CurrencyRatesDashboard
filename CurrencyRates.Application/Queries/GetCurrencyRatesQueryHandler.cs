using CurrencyRates.Application.DTOs;
using CurrencyRates.Application.Interfaces;
using MediatR;

namespace CurrencyRates.Application.Queries;

public class GetCurrencyRatesQueryHandler : IRequestHandler<GetCurrencyRatesQuery, IReadOnlyList<CurrencyRateDto>>
{
    private readonly ICurrencyRateService _service;
    
    public GetCurrencyRatesQueryHandler(ICurrencyRateService service)
    {
        _service = service;
    }
    public async Task<IReadOnlyList<CurrencyRateDto>> Handle(GetCurrencyRatesQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetAllAsync(cancellationToken);
    }
}