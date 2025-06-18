using CSharpFunctionalExtensions;
using CurrencyRates.Domain.Common;
using CurrencyRates.Domain.Currency.ValueObjects.Auth;

namespace CurrencyRates.Domain.Currency.Aggregates.Auth;

public class User : AggregateRoot
{
   public Email Email {get; private set;}
   public PasswordHash PasswordHash {get; private set;}
   public Role Role {get; private set;}
   
   private readonly List<RefreshToken> _refreshTokens = new();
   
   public IReadOnlyList<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();
   
   private User() {}

   private User(Guid id, Email email, PasswordHash passwordHash, Role role)
   {
      ID = id;
      Email = email;
      PasswordHash = passwordHash;
      Role = role;
   }

   public static Result<User> Register(string email, string password, string role)
   {
      if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
         return Result.Failure<User>("Invalid email format");
      if(!email.Contains("@"))
         return Result.Failure<User>("Invalid email format");
      if(role == null) throw new ArgumentNullException(nameof(role));
      
      var emailResult = Email.Create(email);
      if(emailResult.IsFailure) return Result.Failure<User>("Invalid email format");

      var passwordHash = PasswordHash.FromPlainText(password);
      var roleObj = Role.From(role);
      
      return Result.Success(new User(Guid.NewGuid(), emailResult.Value, passwordHash, roleObj));
   }
   public static Result<User> Create(string ID,string email, string password, string role)
   {
      if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
         return Result.Failure<User>("Invalid email format");
      if(role == null) throw new ArgumentNullException(nameof(role));
      
      var emailResult = Email.Create(email);
      if(emailResult.IsFailure) return Result.Failure<User>("Invalid email format");

      var passwordHash = new PasswordHash(password);
      var roleObj = Role.From(role);
      
      return new User(Guid.Parse(ID), emailResult.Value, passwordHash, roleObj);
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