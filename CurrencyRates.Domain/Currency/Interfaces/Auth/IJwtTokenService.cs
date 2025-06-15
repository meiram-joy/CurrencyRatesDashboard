using CurrencyRates.Domain.Currency.Aggregates.Auth;
using CurrencyRates.Domain.Currency.ValueObjects.Auth;

namespace CurrencyRates.Domain.Currency.Services;

public interface IJwtTokenService
{
    string GenerateAccessToken(User user);
    RefreshToken GenerateRefreshToken();
}