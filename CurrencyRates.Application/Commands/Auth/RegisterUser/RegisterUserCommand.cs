using CSharpFunctionalExtensions;
using CurrencyRates.Application.DTOs.Auth;
using MediatR;

namespace CurrencyRates.Application.Commands.Auth.RegisterUser;

public record RegisterUserCommand(string Email, string Password, string Role, string PhoneNumber,string FirstName,string LastName) : IRequest<Result<AuthResultDto>>;