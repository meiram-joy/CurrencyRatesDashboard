using CSharpFunctionalExtensions;
using CurrencyRates.Application.DTOs.Auth;
using CurrencyRates.Domain.Currency.ValueObjects.Auth;
using MediatR;

namespace CurrencyRates.Application.Commands.Auth.LoginUser;

public record LoginUserCommand(Email Email, string Password) : IRequest<Result<AuthResultDto>>;