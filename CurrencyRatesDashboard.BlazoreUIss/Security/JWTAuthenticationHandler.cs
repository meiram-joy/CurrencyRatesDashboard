using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace CurrencyRatesDashboard.BlazoreUIss.Security;

public class JWTAuthenticationHandler : AuthenticationHandler<CusomOption>
{
    public JWTAuthenticationHandler(IOptionsMonitor<CusomOption> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    public JWTAuthenticationHandler(IOptionsMonitor<CusomOption> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            var token = Request.Cookies["AccessToken"];
            if (string.IsNullOrEmpty(token))
                return AuthenticateResult.NoResult();
            var readJWT = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var identity = new ClaimsIdentity(readJWT.Claims, "JWT");
            var principal = new ClaimsPrincipal(identity);
        
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
        catch (Exception e)
        {
            return AuthenticateResult.NoResult();
        }
    }
    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.Redirect("/");
        return Task.CompletedTask;
    }

    protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        Response.Redirect("/accessdenied");
        return Task.CompletedTask;
    }

} 
public class CusomOption : AuthenticationSchemeOptions
{

}

