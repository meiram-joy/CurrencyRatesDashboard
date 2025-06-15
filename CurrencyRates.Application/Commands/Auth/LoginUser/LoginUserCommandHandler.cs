using CSharpFunctionalExtensions;
using CurrencyRates.Application.Commands.Auth.RefreshToken;
using CurrencyRates.Application.DTOs.Auth;
using CurrencyRates.Domain.Currency.Interfaces.Auth;
using CurrencyRates.Domain.Currency.Services;
using MediatR;

namespace CurrencyRates.Application.Commands.Auth.LoginUser;

public class LoginUserCommandHandler :  IRequestHandler<LoginUserCommand, Result<AuthResultDto>>
{
private readonly IUserRepository _userRepository;
private readonly IJwtTokenService _jwtTokenService;
private readonly IRefreshTokenRepository _refreshTokenRepository;

    public LoginUserCommandHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<Result<AuthResultDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null || !user.CheckPassword(request.Password))
            return Result.Failure<AuthResultDto>("Invalid credentials");

        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        user.AddRefreshToken(refreshToken);

        await _userRepository.UpdateAsync(user);
        await _refreshTokenRepository.SaveAsync(user.Id, refreshToken.Token, refreshToken.Expires);

        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        return Result.Success(new AuthResultDto(accessToken, refreshToken.Token));
    }
}