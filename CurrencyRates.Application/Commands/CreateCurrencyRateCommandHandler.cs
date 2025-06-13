using AutoMapper;
using CSharpFunctionalExtensions;
using CurrencyRates.Domain.Currency.Aggregates;
using CurrencyRates.Domain.Currency.Entities;
using CurrencyRates.Domain.Currency.Interfaces;
using CurrencyRates.Domain.Currency.ValueObjects;
using MediatR;

namespace CurrencyRates.Application.Commands;

public class CreateCurrencyRateCommandHandler : IRequestHandler<CreateCurrencyRateCommand, Result>
{
    private readonly ICurrencyRateRepository _repository;
    private readonly IMapper _mapper;

    public CreateCurrencyRateCommandHandler(ICurrencyRateRepository repository, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
    }
    
    public async Task<Result> Handle(CreateCurrencyRateCommand request, CancellationToken cancellationToken)
    {
        var dto = request.currencyRate;
        
        var currencyCodeResult  = CurrencyCode.Create(dto.CurrencyCode);
        if (currencyCodeResult.IsFailure)
            return Result.Failure(currencyCodeResult.Error);
        
        var currencyCode = currencyCodeResult.Value;
        
        var existing  = await _repository.GetByCodeAsync(currencyCode,cancellationToken);
        
        if (existing is null)
        {
            var createResult = CurrencyRateAggregate.Create(currencyCode, dto.CurrencyName, dto.Rate, dto.Date);
            if (createResult.IsFailure)
                return Result.Failure(createResult.Error);
            
            await _repository.AddAsync(createResult.Value, cancellationToken);
        }
        else
        {
            existing.AddOrUpdateQuote(currencyCode, dto.Rate, dto.Date);
            await _repository.UpdateAsync(existing, cancellationToken);
        }
        return Result.Success();
    }
}