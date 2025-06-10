using CSharpFunctionalExtensions;

namespace CurrencyRates.Domain.Currency.ValueObjects;

public sealed class CurrencyCode : ValueObject
{
    public string Code { get; }

    private CurrencyCode(string code)
    {
            Code = code;
    }
    
    public static CurrencyCode Create(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Currency code cannot be null or empty.", nameof(code));
        }

        return new CurrencyCode(code);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }
    
    public override string ToString() => Code;
}