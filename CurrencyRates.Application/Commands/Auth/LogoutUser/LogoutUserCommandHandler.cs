using CSharpFunctionalExtensions;
using CurrencyRates.Application.DTOs.Auth;
using CurrencyRates.Domain.Currency.Interfaces.Auth;
using MediatR;

namespace CurrencyRates.Application.Commands.Auth.LogoutUser;

public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand,Result<LogoutResultDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthDomainService _authDomainService;
    
    public LogoutUserCommandHandler(IUserRepository userRepository, IAuthDomainService authDomainService)
    {
        _userRepository = userRepository;
        _authDomainService = authDomainService;
    }
    
    public async Task<Result<LogoutResultDto>> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            return Result.Failure<LogoutResultDto>("Пользователь не найден");
        
        var refreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == request.RefreshToken);
        if (refreshToken == null)
            return Result.Failure<LogoutResultDto>("Неверный токен");
        
        var result = await _authDomainService.LogoutAsync(user,refreshToken, cancellationToken);
        if (result.IsFailure)
            return Result.Failure<LogoutResultDto>(result.Error);

        return Result.Success(new LogoutResultDto { Success = true });
    }
}