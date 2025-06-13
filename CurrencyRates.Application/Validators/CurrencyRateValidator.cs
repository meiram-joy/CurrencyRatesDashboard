using CurrencyRates.Application.DTOs;
using FluentValidation;

namespace CurrencyRates.Application.Validators;

public class CurrencyRateValidator : AbstractValidator<CurrencyRateDto>
{
    public CurrencyRateValidator()
    {
        RuleFor(currencyRate => currencyRate.CurrencyCode)
            .NotEmpty().WithMessage("Currency Code cannot be empty.");
        RuleFor(currencyRate => currencyRate.Rate)
            .GreaterThan(0).WithMessage("Rate must be greater than zero.");
        RuleFor(currencyRate => currencyRate.Date)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Rate date cannot be in the future.");
        RuleFor(currencyRate => currencyRate.Date)
            .GreaterThanOrEqualTo(DateTime.Now.AddYears(-1)).WithMessage("Rate date cannot be older than one year.");
    }
}