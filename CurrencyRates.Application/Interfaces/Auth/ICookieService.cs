using Microsoft.AspNetCore.Http;

namespace CurrencyRates.Application.Interfaces.Auth;

public interface ICookieService
{
    void SetRefreshTokenAndAccessTokenCookie(string refreshToken,string accessToken, HttpResponse response);
    void DeleteRefreshTokenCookie(HttpResponse response);
    
}