using CSharpFunctionalExtensions;

namespace CurrencyRates.Domain.Currency.ValueObjects.Auth;

public sealed  class Role: ValueObject<Role>
{
    private string Name { get; set; }
    
    private Role(string name) => Name = name;
    
    public static Role Admin => new Role("Admin");
    public static Role User => new Role("User");
    public static Role From(string name) => new Role(name);
    protected override bool EqualsCore(Role other) => Name == other.Name;

    protected override int GetHashCodeCore() => Name.GetHashCode();
    
}