using AutoMapper;
using CurrencyRates.Application.DTOs;
using CurrencyRates.Application.Interfaces;
using CurrencyRates.Domain.Currency.Interfaces;

namespace CurrencyRates.Application.Services;

public class CurrencyRateService : ICurrencyRateService
{
    private readonly ICurrencyRateRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICurrencyRateApiClient _apiClient;


    public CurrencyRateService(ICurrencyRateRepository repository, IMapper mapper, ICurrencyRateApiClient apiClient)
    {
        _repository = repository;
        _mapper = mapper;
        _apiClient = apiClient;
    }
    
    public async Task<IReadOnlyList<CurrencyRateDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var domainRates = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<CurrencyRateDto>>(domainRates);
    }

    public async Task<IReadOnlyList<CurrencyRateDto>> RefreshRatesAsync(CancellationToken cancellationToken = default)
    {
        var newRates = await _apiClient.GetLatestRatesAsync();

        foreach (var rate in newRates)
        {
            var existing = await _repository.GetByCodeAsync(rate.CurrencyCode, cancellationToken);
            if (existing is null)
            {
                await _repository.AddAsync(rate, cancellationToken);
            }
            else
            {
                existing.AddOrUpdateQuote(
                rate.CurrencyCode,
                rate.Rate,
                rate.RetrievedAt
                );
                await _repository.UpdateAsync(rate, cancellationToken);
            }
        }

        var updatedRates = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<CurrencyRateDto>>(updatedRates);
    }
}