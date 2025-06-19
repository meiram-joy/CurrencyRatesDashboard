using CurrencyRates.Application.Commands.Auth.LoginUser;
using CurrencyRates.Application.Commands.Auth.LogoutUser;
using CurrencyRates.Application.Commands.Auth.RefreshToken;
using CurrencyRates.Application.DTOs.Auth;
using CurrencyRates.Application.Interfaces.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRatesDashboard.BlazoreUIss.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICookieService _cookieService;

    public AuthController(IMediator mediator, ICookieService cookieService)
    {
        _mediator = mediator;
        _cookieService = cookieService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result.Error);
        
        _cookieService.SetRefreshTokenAndAccessTokenCookie(result.Value.RefreshToken,result.Value.AccessToken,Response);
        return Ok(new { accessToken = result.Value.AccessToken });
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result.Error);
        
        _cookieService.SetRefreshTokenAndAccessTokenCookie(result.Value.RefreshToken,result.Value.AccessToken,Response);
        return Ok(new { accessToken = result.Value.AccessToken });
    }
    
    [Authorize]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub").Value;
        if (!Guid.TryParse(userId, out var guid))
            return Unauthorized("Invalid token");
        
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            return BadRequest("Refresh token not found in cookies.");
        
        var command = new LogoutUserCommand(guid, refreshToken);
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result.Error);
        
        _cookieService.SetRefreshTokenAndAccessTokenCookie(result.Value.RefreshToken,result.Value.AccessToken,Response);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub")?.Value;

        if (!Guid.TryParse(userId, out var guid))
            return Unauthorized("Invalid token");
        
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            return BadRequest("Refresh token not found in cookies.");
        
        var command = new LogoutUserCommand(guid, refreshToken);
        var result = await _mediator.Send(command, cancellationToken);
        
        _cookieService.DeleteRefreshTokenCookie(Response);

        return result.IsSuccess ? Ok( new {success = true}) : BadRequest(result.Error);
    }
}