using CSharpFunctionalExtensions;
using CurrencyRates.Domain.Common;
using CurrencyRates.Domain.Currency.ValueObjects.Auth;

namespace CurrencyRates.Domain.Currency.Aggregates.Auth;

public class User : AggregateRoot
{
   public Email Email {get; private set;}
   public PasswordHash PasswordHash {get; private set;}
   public Role Role {get; private set;}

   public FullName FullName { get; set; }
   
   public PhoneNumber PhoneNumber { get; set; }
   
   private readonly List<RefreshToken> _refreshTokens = new();
   
   public IReadOnlyList<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();
   
   private User() {}

   private User(Guid id, Email email, PasswordHash passwordHash, Role role, FullName fullName, PhoneNumber phoneNumber)
   {
      ID = id;
      Email = email;
      PasswordHash = passwordHash;
      Role = role;
      FullName = fullName;
      PhoneNumber = phoneNumber;
   }

   public static Result<User> Register(string email, string password, string role, string firstName,string lastName , string phoneNumber)
   {
      if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
         return Result.Failure<User>("Invalid email format");
      if(!email.Contains("@"))
         return Result.Failure<User>("Invalid email format");
      if(role == null) throw new ArgumentNullException(nameof(role));
      if(string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
         return Result.Failure<User>("First name and last name cannot be empty or null.");
      if(string.IsNullOrWhiteSpace(phoneNumber))
         return Result.Failure<User>("Phone number cannot be empty or null.");
      
      var fullNameResult = FullName.Create(firstName, lastName);
      if(fullNameResult.IsFailure) return Result.Failure<User>(fullNameResult.Error);
      
      var phoneNumberResult = PhoneNumber.Create(phoneNumber);
      if(phoneNumberResult.IsFailure) return Result.Failure<User>(phoneNumberResult.Error);
      
      var emailResult = Email.Create(email);
      if(emailResult.IsFailure) return Result.Failure<User>("Invalid email format");

      var passwordHash = PasswordHash.FromPlainText(password);
      var roleObj = Role.From(role);
      
      return Result.Success(new User(Guid.NewGuid(), emailResult.Value, passwordHash, roleObj, fullNameResult.Value, phoneNumberResult.Value));
   }
   public static Result<User> Create(string ID, string email, string password, string role, string firstName, string lastName , string phoneNumber)
   {
      if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
         return Result.Failure<User>("Invalid email format");
      if(role == null) throw new ArgumentNullException(nameof(role));
      if(string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
         return Result.Failure<User>("First name and last name cannot be empty or null.");
      if(string.IsNullOrWhiteSpace(phoneNumber))
         return Result.Failure<User>("Phone number cannot be empty or null.");
      
      var fullNameResult = FullName.Create(firstName, lastName);
      if(fullNameResult.IsFailure) return Result.Failure<User>(fullNameResult.Error);
      
      var phoneNumberResult = PhoneNumber.Create(phoneNumber);
      if(phoneNumberResult.IsFailure) return Result.Failure<User>(phoneNumberResult.Error);
      
      var emailResult = Email.Create(email);
      if(emailResult.IsFailure) return Result.Failure<User>("Invalid email format");

      var passwordHash = new PasswordHash(password);
      var roleObj = Role.From(role);
      
      return new User(Guid.Parse(ID), emailResult.Value, passwordHash, roleObj, fullNameResult.Value, phoneNumberResult.Value);
   }
   public bool CheckPassword(string password) => PasswordHash.Verify(password);

   public void ChangePassword(string newPassword)
   {
      PasswordHash = PasswordHash.FromPlainText(newPassword);
   }
   public void AddRefreshToken(RefreshToken token)
   {
      _refreshTokens.Add(token);
   }
   public bool HasValidRefreshToken(string token)
   {
      return _refreshTokens.Any(x => x.Token == token && !x.IsExpired());
   }
   public void RevokeRefreshToken(string token)
   {
      _refreshTokens.RemoveAll(x => x.Token == token);
   }
}