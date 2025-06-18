using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CurrencyRates.Domain.Currency.Aggregates.Auth;
using CurrencyRates.Domain.Currency.Services;
using CurrencyRates.Domain.Currency.ValueObjects.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CurrencyRates.Infrastructure.Services.Auth;

public class JwtTokenGenerator : IJwtTokenService
{
    private readonly JwtSettings _settings;

    public JwtTokenGenerator(IOptions<JwtSettings> options)
    {
        _settings = options.Value;
    }
    

    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.ID.ToString()),
            new Claim(JwtRegisteredClaimNames.Email,  user.Email.Value),
            new Claim(ClaimTypes.Role, user.Role.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    RefreshToken IJwtTokenService.GenerateRefreshToken()
    {
        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        return new RefreshToken(token, DateTime.UtcNow.AddDays(7));
    }
}

    public class JwtSettings
    {
        public const string SectionName = "JwtSettings";

        public string Secret { get; init; } = string.Empty;
        public int ExpiryMinutes { get; init; }
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
    }
