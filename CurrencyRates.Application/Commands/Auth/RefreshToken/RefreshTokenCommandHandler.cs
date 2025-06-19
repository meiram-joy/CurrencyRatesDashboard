using CSharpFunctionalExtensions;
using CurrencyRates.Domain.Currency.Interfaces.Auth;
using CurrencyRates.Domain.Currency.Services;
using MediatR;

namespace CurrencyRates.Application.Commands.Auth.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand,  Result<(string AccessToken, string RefreshToken)>>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthDomainService _authDomainService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RefreshTokenCommandHandler(IUserRepository userRepository, 
        IAuthDomainService authDomainService,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _authDomainService = authDomainService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<Result<(string, string)>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            return Result.Failure<(string, string)>("User not found");
        
        var refreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == request.RefreshToken);
        if (refreshToken == null)
            return Result.Failure<(string, string)>("Invalid refresh token");
        try
        {
            var (accessToken, newRefreshToken) = await _authDomainService.RefreshAsync(user, refreshToken, cancellationToken);
            return Result.Success((accessToken, newRefreshToken.Token));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Result.Failure<(string, string)>(ex.Message);
        }
    }
}