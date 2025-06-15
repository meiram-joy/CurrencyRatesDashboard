using CSharpFunctionalExtensions;
using CurrencyRates.Domain.Currency.Aggregates.Auth;
using MediatR;

namespace CurrencyRates.Application.Queries.Auth.GetUser;

public record GetUserByIdQuery(Guid UserId) : IRequest<Result<User>>;