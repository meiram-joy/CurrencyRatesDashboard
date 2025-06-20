using CSharpFunctionalExtensions;
using CurrencyRates.Application.Commands.Auth.LoginUser;
using CurrencyRates.Application.DTOs.Auth;
using CurrencyRates.Domain.Currency.Aggregates.Auth;
using CurrencyRates.Domain.Currency.Interfaces.Auth;
using CurrencyRates.Domain.Currency.Services;
using MediatR;

namespace CurrencyRates.Application.Commands.Auth.RegisterUser;

public class RegisterUserCommandHandler :  IRequestHandler<RegisterUserCommand, Result<AuthResultDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RegisterUserCommandHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService, IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<Result<AuthResultDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userResult = User.Register(request.Email, request.Password, request.Role, request.FirstName, request.LastName, request.PhoneNumber);
        if (userResult.IsFailure)
            return Result.Failure<AuthResultDto>(userResult.Error);

        var user = userResult.Value;
        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        user.AddRefreshToken(refreshToken);

        await _userRepository.AddAsync(user);
        await _refreshTokenRepository.SaveAsync(user.ID, refreshToken, cancellationToken);

        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        return Result.Success(new AuthResultDto(accessToken, refreshToken.Token));
    }
}