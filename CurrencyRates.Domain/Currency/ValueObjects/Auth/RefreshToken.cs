using CSharpFunctionalExtensions;

namespace CurrencyRates.Domain.Currency.ValueObjects.Auth;

public class RefreshToken(string token, DateTime expires) : ValueObject<RefreshToken>
{
    public string Token { get; private set; } = token;
    public DateTime Expires { get; private set; } = expires;

    public static RefreshToken Create(string token, DateTime expires)
    {
        return new RefreshToken(token, expires);
    }
    public bool IsExpired() => DateTime.UtcNow >= Expires;
    protected override bool EqualsCore(RefreshToken other) => 
        Token == other.Token && Expires == other.Expires;

    protected override int GetHashCodeCore() => HashCode.Combine(Token, Expires);
}