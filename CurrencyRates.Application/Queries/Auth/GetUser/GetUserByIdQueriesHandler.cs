using CSharpFunctionalExtensions;
using CurrencyRates.Domain.Currency.Aggregates.Auth;
using CurrencyRates.Domain.Currency.Interfaces.Auth;
using MediatR;

namespace CurrencyRates.Application.Queries.Auth.GetUser;

public class GetUserByIdQueriesHandler: IRequestHandler<GetUserByIdQuery, Result<User>>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueriesHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        return user is null
            ? Result.Failure<User>("User not found")
            : Result.Success(user);
    }
}