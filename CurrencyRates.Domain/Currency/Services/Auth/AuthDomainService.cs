using CSharpFunctionalExtensions;
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
    

    public async Task<(string accessToken, RefreshToken refreshToken)> RefreshAsync(User user, RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        if (!user.HasValidRefreshToken(refreshToken.Token))
            throw new UnauthorizedAccessException("Invalid refresh token");

        if (!await _refreshTokenRepository.ValidateAsync(user.ID,refreshToken, cancellationToken))
            throw new UnauthorizedAccessException("Недействительный refresh token");
        
        var newAccessToken = _jwtTokenService.GenerateAccessToken(user);
        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

        user.RevokeRefreshToken(refreshToken.Token);
        user.AddRefreshToken(newRefreshToken);

        await _refreshTokenRepository.InvalidateAsync(user.ID, refreshToken,cancellationToken);
        await _refreshTokenRepository.SaveAsync(user.ID, newRefreshToken, cancellationToken);

        return (newAccessToken, newRefreshToken);
    }

    public async Task<Result> LogoutAsync(User user, RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        if (!user.HasValidRefreshToken(refreshToken.Token)) 
              throw new UnauthorizedAccessException("Invalid refresh token");
        user.RevokeRefreshToken(refreshToken.Token);
        try
        {
            await _refreshTokenRepository.InvalidateAsync(user.ID, refreshToken,cancellationToken);
            return Result.Success();
        }
        catch (UnauthorizedAccessException ex)
        {
            //Todo: log exception
            return Result.Failure(ex.Message);
        }
    }
}

