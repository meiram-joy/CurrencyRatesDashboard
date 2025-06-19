using CSharpFunctionalExtensions;
using CurrencyRates.Application.DTOs.Auth;
using MediatR;

namespace CurrencyRates.Application.Commands.Auth.LogoutUser;

public record class LogoutUserCommand(Guid UserId, string RefreshToken) : IRequest<Result<LogoutResultDto>>;