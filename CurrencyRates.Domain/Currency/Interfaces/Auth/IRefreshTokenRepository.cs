using CurrencyRates.Domain.Currency.ValueObjects.Auth;

namespace CurrencyRates.Domain.Currency.Interfaces.Auth;

public interface IRefreshTokenRepository
{
    Task SaveAsync(Guid userId, RefreshToken refreshToken, CancellationToken cancellationToken = default);
    Task<bool> ValidateAsync(Guid userId, RefreshToken refreshToken, CancellationToken cancellationToken = default);
    Task InvalidateAsync(Guid userId, RefreshToken refreshToken, CancellationToken cancellationToken = default);
}