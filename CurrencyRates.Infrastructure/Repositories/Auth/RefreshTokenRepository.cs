using CurrencyRates.Domain.Currency.Interfaces.Auth;
using CurrencyRates.Domain.Currency.ValueObjects.Auth;

namespace CurrencyRates.Infrastructure.Repositories.Auth;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    public Task SaveAsync(Guid userId, string refreshToken, DateTime expires)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidateAsync(Guid userId, string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task InvalidateAsync(Guid userId, string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task<RefreshToken?> GetAsync(Guid userId, string refreshToken)
    {
        throw new NotImplementedException();
    }
}