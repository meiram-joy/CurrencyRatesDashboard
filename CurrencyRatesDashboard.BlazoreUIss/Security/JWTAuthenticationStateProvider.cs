using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CurrencyRatesDashboard.BlazoreUIss.Services.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace CurrencyRatesDashboard.BlazoreUIss.Security;

public class JWTAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly AccessTokenService _accessTokenService;

    public JWTAuthenticationStateProvider(AccessTokenService accessTokenService)
    {
        _accessTokenService = accessTokenService;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await _accessTokenService.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                return await MarkAsUnauthenticated();
            
            var readJWT = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var identity = new ClaimsIdentity(readJWT.Claims, "JWT");
            var principal = new ClaimsPrincipal(identity);
            
            return await Task.FromResult(new AuthenticationState(principal));
        }
        catch (Exception e)
        {
            return await MarkAsUnauthenticated();
        }
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