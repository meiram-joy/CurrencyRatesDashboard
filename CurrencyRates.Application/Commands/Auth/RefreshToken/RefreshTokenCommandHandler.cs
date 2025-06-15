using CSharpFunctionalExtensions;
using CurrencyRates.Domain.Currency.Interfaces.Auth;
using CurrencyRates.Domain.Currency.Services;
using MediatR;

namespace CurrencyRates.Application.Commands.Auth.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<string>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshTokenCommandHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<string>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null || !user.HasValidRefreshToken(request.RefreshToken))
            return Result.Failure<string>("Invalid refresh token");

        var newAccessToken = _jwtTokenService.GenerateAccessToken(user);
        return Result.Success(newAccessToken);
    }
}