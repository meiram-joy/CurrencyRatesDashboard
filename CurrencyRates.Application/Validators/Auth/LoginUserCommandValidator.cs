using CurrencyRates.Application.Commands.Auth.LoginUser;
using CurrencyRates.Application.Commands.Auth.RefreshToken;
using CurrencyRates.Application.Commands.Auth.RegisterUser;
using FluentValidation;

namespace CurrencyRates.Application.Validators.Auth;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email.Value)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}
