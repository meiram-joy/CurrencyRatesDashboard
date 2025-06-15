using CurrencyRates.Domain.Currency.Aggregates.Auth;
using CurrencyRates.Domain.Currency.Interfaces.Auth;
using CurrencyRates.Domain.Currency.Services;
using CurrencyRates.Domain.Currency.ValueObjects.Auth;

public class AuthDomainService : IAuthDomainService
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthDomainService(
        IJwtTokenService jwtTokenService,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _jwtTokenService = jwtTokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<(string accessToken, RefreshToken refreshToken)> LoginAsync(User user, string password)
    {
        if (!user.CheckPassword(password))
            throw new UnauthorizedAccessException("Invalid credentials");

        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        user.AddRefreshToken(refreshToken);
        await _refreshTokenRepository.SaveAsync(user.Id, refreshToken.Token, refreshToken.Expires);

        return (accessToken, refreshToken);
    }

    public async Task<(string accessToken, RefreshToken refreshToken)> RefreshAsync(User user, string refreshToken)
    {
        if (!user.HasValidRefreshToken(refreshToken))
            throw new UnauthorizedAccessException("Invalid refresh token");

        var newAccessToken = _jwtTokenService.GenerateAccessToken(user);
        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

        user.RevokeRefreshToken(refreshToken);
        user.AddRefreshToken(newRefreshToken);

        await _refreshTokenRepository.InvalidateAsync(user.Id, refreshToken);
        await _refreshTokenRepository.SaveAsync(user.Id, newRefreshToken.Token, newRefreshToken.Expires);

        return (newAccessToken, newRefreshToken);
    }

    public async Task LogoutAsync(User user, string refreshToken)
    {
        user.RevokeRefreshToken(refreshToken);
        await _refreshTokenRepository.InvalidateAsync(user.Id, refreshToken);
    }
}

