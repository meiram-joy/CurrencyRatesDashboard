using CSharpFunctionalExtensions;

namespace CurrencyRates.Domain.Currency.ValueObjects.Auth;

public class PasswordHash : ValueObject<PasswordHash>
{
    public string Hash { get; private set; }
    public PasswordHash(string hash) => Hash = hash;

    public static PasswordHash FromPlainText(string password)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        return new PasswordHash(hash);
    }

    public bool Verify(string password)
    {
       return  BCrypt.Net.BCrypt.Verify(password, Hash);
    }

    protected override bool EqualsCore(PasswordHash other) => Hash == other.Hash;

    protected override int GetHashCodeCore() => Hash.GetHashCode();
}