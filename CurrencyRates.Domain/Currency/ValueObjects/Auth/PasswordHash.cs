using CSharpFunctionalExtensions;

namespace CurrencyRates.Domain.Currency.ValueObjects.Auth;

public class PasswordHash : ValueObject<PasswordHash>
{
    private string Hash { get; set; }
    public PasswordHash(string hash) => Hash = hash;

    public static PasswordHash FromPlainText(string password)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        return new PasswordHash(hash);
    }
    public bool Verify(string hash) => BCrypt.Net.BCrypt.Verify(Hash, hash);
    protected override bool EqualsCore(PasswordHash other) => Hash == other.Hash;

    protected override int GetHashCodeCore() => Hash.GetHashCode();
}