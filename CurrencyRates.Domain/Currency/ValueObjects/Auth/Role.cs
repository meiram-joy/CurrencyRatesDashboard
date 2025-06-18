using CSharpFunctionalExtensions;

namespace CurrencyRates.Domain.Currency.ValueObjects.Auth;

public sealed  class Role: ValueObject<Role>
{
    public string Name { get; private set; }
    
    private Role(string name) => Name = name;
    
    public static Role Admin => new Role("Admin");
    public static Role User => new Role("User");
    public static Role From(string name) => new Role(name);
    protected override bool EqualsCore(Role other) => Name == other.Name;
    
    public static Result<Role> Create(string value)
    {
        if(string.IsNullOrWhiteSpace(value) )
            return Result.Failure<Role>("Invalid Role format");
        return Result.Success((new Role(value.Trim().ToLowerInvariant())));
    }

    protected override int GetHashCodeCore() => Name.GetHashCode();
    
}