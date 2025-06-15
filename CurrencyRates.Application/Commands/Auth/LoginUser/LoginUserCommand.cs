using CSharpFunctionalExtensions;
using CurrencyRates.Application.DTOs.Auth;
using MediatR;

namespace CurrencyRates.Application.Commands.Auth.LoginUser;

public record RegisterUserCommand(string Email, string Password, string Role) : IRequest<Result<AuthResultDto>>;