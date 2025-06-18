using CurrencyRates.Domain.Currency.Aggregates.Auth;
using CurrencyRates.Domain.Currency.ValueObjects.Auth;

namespace CurrencyRates.Domain.Currency.Interfaces.Auth;

public interface IAuthDomainService
{
    Task<(string accessToken, RefreshToken refreshToken)> LoginAsync(User user, string password, CancellationToken cancellationToken = default);
    Task<(string accessToken, RefreshToken refreshToken)> RefreshAsync(User user, RefreshToken refreshToken, CancellationToken cancellationToken = default);
    Task LogoutAsync(User user, RefreshToken refreshToken, CancellationToken cancellationToken = default);
}