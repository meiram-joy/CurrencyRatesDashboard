using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CurrencyRatesDashboard.BlazoreUIss.Services.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace CurrencyRatesDashboard.BlazoreUIss.Security;

public class JWTAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IJSRuntime _jsRuntime;

    public JWTAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor, IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        _httpContextAccessor = httpContextAccessor;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user?.Identity is { IsAuthenticated: true } && TokenIsExpired(user))
            {
                var refreshed = await _jsRuntime.InvokeAsync<bool>("authRefresh");
                if (refreshed)
                {
                    user = _httpContextAccessor.HttpContext?.User;
                }
                else
                {
                    return await  MarkAsUnauthenticated();
                }
            }
            return new AuthenticationState(user ?? new ClaimsPrincipal(new ClaimsIdentity()));
        }
        catch (Exception e)
        {
            return await MarkAsUnauthenticated();
        }
    }
    private bool TokenIsExpired(ClaimsPrincipal user)
    {
        var exp = user.FindFirst("exp")?.Value;
        if (exp == null) return true;

        var expiryUnix = long.Parse(exp);
        var expiryDate = DateTimeOffset.FromUnixTimeSeconds(expiryUnix);
        return DateTimeOffset.UtcNow > expiryDate;
    }
    private async Task<AuthenticationState>  MarkAsUnauthenticated()
    {
        try
        {
            var state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }
        catch (Exception e)
        {
            return  new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
}

