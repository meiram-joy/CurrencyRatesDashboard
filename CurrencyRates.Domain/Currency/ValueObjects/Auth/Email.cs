using CSharpFunctionalExtensions;

namespace CurrencyRates.Domain.Currency.ValueObjects.Auth;

public sealed  class Email : ValueObject<Email>
{
    public string Value { get; private set; }

    private Email(string value) => Value = value;

    public static Result<Email> Create(string value)
    {
        if(string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
            return Result.Failure<Email>("Invalid email format");
        return Result.Success((new Email(value.Trim().ToLowerInvariant())));
    }
    protected override bool EqualsCore(Email other) => Value == other.Value;

    protected override int GetHashCodeCore() => Value.GetHashCode();
}