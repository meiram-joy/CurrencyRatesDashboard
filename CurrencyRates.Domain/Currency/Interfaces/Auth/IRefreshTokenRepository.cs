using CurrencyRates.Domain.Currency.ValueObjects.Auth;

namespace CurrencyRates.Domain.Currency.Interfaces.Auth;

public interface IRefreshTokenRepository
{
    Task SaveAsync(Guid userId, string refreshToken, DateTime expires);
    Task<bool> ValidateAsync(Guid userId, string refreshToken);
    Task InvalidateAsync(Guid userId, string refreshToken);
    Task<RefreshToken?> GetAsync(Guid userId, string refreshToken);
}