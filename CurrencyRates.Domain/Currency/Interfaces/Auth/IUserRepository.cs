using CurrencyRates.Domain.Currency.Aggregates;
using CurrencyRates.Domain.Currency.Aggregates.Auth;

namespace CurrencyRates.Domain.Currency.Interfaces.Auth;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}