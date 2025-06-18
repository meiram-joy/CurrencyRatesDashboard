using CurrencyRates.Application.Commands.Auth.LoginUser;
using CurrencyRates.Application.Commands.Auth.RefreshToken;
using CurrencyRates.Domain.Currency.Interfaces.Auth;
using CurrencyRates.Domain.Currency.ValueObjects.Auth;
using CurrencyRates.Infrastructure.Services.Auth;
using CurrencyRatesDashboard.BlazoreUIss.Models;
using CurrencyRatesDashboard.BlazoreUIss.Models.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRatesDashboard.BlazoreUIss.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    
    [Authorize(Policy = "AdminOnly")]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result.Error);
        
        SetRefreshTokenCookie(result.Value.RefreshToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] string refreshToken, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub")?.Value;

        if (!Guid.TryParse(userId, out var guid))
            return Unauthorized("Invalid token");

        var command = new RefreshTokenCommand(guid, refreshToken);
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
    public void SetRefreshTokenCookie(string refreshToken)
    {
        Response.Cookies.Append(
            "refreshToken",
            refreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
    }
}