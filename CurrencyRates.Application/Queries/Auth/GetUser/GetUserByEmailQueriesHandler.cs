using CSharpFunctionalExtensions;
using CurrencyRates.Domain.Currency.Aggregates.Auth;
using CurrencyRates.Domain.Currency.Interfaces.Auth;
using MediatR;

namespace CurrencyRates.Application.Queries.Auth.GetUser;

public class GetUserByEmailQueriesHandler : IRequestHandler<GetUserByEmailQuery, Result<User>>
{
    private readonly IUserRepository _userRepository;

    public GetUserByEmailQueriesHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<User>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        return user is null
            ? Result.Failure<User>("User not found")
            : Result.Success(user);
    }
}