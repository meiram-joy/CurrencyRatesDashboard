using CurrencyRates.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Http;

namespace CurrencyRates.Infrastructure.Services.Auth;

public class CookieService : ICookieService
{
    public void SetRefreshTokenAndAccessTokenCookie(string refreshToken,string accessToken, HttpResponse response)
    {
        response.Cookies.Append(
            "refreshToken",
            refreshToken,
            new CookieOptions
            {
                Path = "/",
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
        response.Cookies.Append(
            "AccessToken",
            accessToken,
            new CookieOptions
            {
                Path = "/",
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(2)
            });
    }

    public void DeleteRefreshTokenCookie(HttpResponse response)
    {
        response.Cookies.Delete("refreshToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict
        });
        response.Cookies.Delete("AccessToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict
        });
    }
}