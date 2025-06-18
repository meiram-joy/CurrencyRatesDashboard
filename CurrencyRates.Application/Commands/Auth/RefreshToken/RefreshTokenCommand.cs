using CSharpFunctionalExtensions;
using MediatR;

namespace CurrencyRates.Application.Commands.Auth.RefreshToken;

public record RefreshTokenCommand(Guid UserId, string RefreshToken) : IRequest<Result<(string AccessToken, string RefreshToken)>>;