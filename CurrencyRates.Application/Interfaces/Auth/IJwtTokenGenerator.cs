using CurrencyRates.Domain.Currency.Aggregates.Auth;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
}