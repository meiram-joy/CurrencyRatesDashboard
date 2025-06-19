using CSharpFunctionalExtensions;
using CurrencyRates.Domain.Currency.Aggregates.Auth;
using CurrencyRates.Domain.Currency.ValueObjects.Auth;

namespace CurrencyRates.Domain.Currency.Interfaces.Auth;

public interface IAuthDomainService
{
    Task<(string accessToken, RefreshToken refreshToken)> RefreshAsync(User user, RefreshToken refreshToken, CancellationToken cancellationToken = default);
    Task<Result> LogoutAsync(User user, RefreshToken refreshToken, CancellationToken cancellationToken = default);
}