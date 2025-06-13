using CurrencyRates.Application.Commands;
using FluentValidation;

namespace CurrencyRates.Application.Validators;

public class CreateCurrencyRateCommandValidator : AbstractValidator<CreateCurrencyRateCommand>
{
    public CreateCurrencyRateCommandValidator()
    {
        RuleFor(x => x.currencyRate).SetValidator(new CurrencyRateValidator());
    }
}