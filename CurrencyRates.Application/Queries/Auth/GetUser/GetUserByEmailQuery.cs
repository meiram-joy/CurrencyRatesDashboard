using CSharpFunctionalExtensions;
using CurrencyRates.Domain.Currency.Aggregates.Auth;
using CurrencyRates.Domain.Currency.ValueObjects.Auth;
using MediatR;

namespace CurrencyRates.Application.Queries.Auth.GetUser;

public record GetUserByEmailQuery(Email Email) : IRequest<Result<User>>;