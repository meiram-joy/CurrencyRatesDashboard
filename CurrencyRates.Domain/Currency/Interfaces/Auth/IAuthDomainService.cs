using CurrencyRates.Domain.Currency.Aggregates.Auth;
using CurrencyRates.Domain.Currency.ValueObjects.Auth;

namespace CurrencyRates.Domain.Currency.Interfaces.Auth;

public interface IAuthDomainService
{
    Task<(string accessToken, RefreshToken refreshToken)> LoginAsync(User user, string password);
    Task<(string accessToken, RefreshToken refreshToken)> RefreshAsync(User user, string refreshToken);
    Task LogoutAsync(User user, string refreshToken);
}