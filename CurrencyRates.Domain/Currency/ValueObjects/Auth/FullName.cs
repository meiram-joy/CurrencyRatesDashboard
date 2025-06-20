using CSharpFunctionalExtensions;

namespace CurrencyRates.Domain.Currency.ValueObjects.Auth;

public class FullName : ValueObject
{
    public string FirstName { get; private set; }
    
    public string LastName { get; private set; }
    
    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
    public static Result<FullName>  Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("First name and last name cannot be empty or null.");

        return Result.Success(new FullName(firstName.Trim(), lastName.Trim()));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }
    public override string ToString() => $"{FirstName} {LastName}";
}