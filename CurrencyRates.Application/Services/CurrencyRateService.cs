using AutoMapper;
using CurrencyRates.Application.DTOs;
using CurrencyRates.Application.Interfaces;
using CurrencyRates.Domain.Currency.Interfaces;

namespace CurrencyRates.Application.Services;

public class CurrencyRateService : ICurrencyRateService
{
    private readonly ICurrencyRateRepository _repository;
    private readonly IMapper _mapper;
    
    public CurrencyRateService(ICurrencyRateRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<IReadOnlyList<CurrencyRateDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var domainRates = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<CurrencyRateDto>>(domainRates);
    }

    public Task RefreshRatesAsync(CancellationToken cancellationToken = default)
    {
        // TODO: получить курсы из внешнего API, обновить в репозитории
        // Например:
        // var newRates = await _externalApiClient.GetLatestRatesAsync();
        // await _repository.ReplaceAllAsync(newRates, cancellationToken);
        
        throw new NotImplementedException("RefreshRatesAsync должен быть реализован при наличии внешнего API клиента.");
    }
}