using CSharpFunctionalExtensions;
using CurrencyRates.Application.DTOs.Auth;
using MediatR;

namespace CurrencyRates.Application.Commands.Auth.RefreshToken;

public record LoginUserCommand(string Email, string Password) : IRequest<Result<AuthResultDto>>;