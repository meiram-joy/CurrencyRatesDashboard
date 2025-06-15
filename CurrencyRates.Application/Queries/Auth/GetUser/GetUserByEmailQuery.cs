using CSharpFunctionalExtensions;
using CurrencyRates.Domain.Currency.Aggregates.Auth;
using MediatR;

namespace CurrencyRates.Application.Queries.Auth.GetUser;

public record GetUserByEmailQuery(string Email) : IRequest<Result<User>>;