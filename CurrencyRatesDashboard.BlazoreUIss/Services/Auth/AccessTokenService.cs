namespace CurrencyRatesDashboard.BlazoreUIss.Services.Auth;

public class AccessTokenService
{
    private readonly CookieService _cookieService;
    private readonly string tokenKey = "AccessToken" ;
    

    public AccessTokenService(CookieService cookieService)
    {
        _cookieService = cookieService;
    }
    
    public async Task SetTokenAsync(string accessToken)
    {
       await _cookieService.SetCookieAsync(tokenKey, accessToken,1);
    }
    
    public async Task RemoveTokenAsync()
    {
      await _cookieService.RemoveCookieAsync(tokenKey);
    }
    
    public async Task<string> GetTokenAsync()
    {
        return await _cookieService.GetCookieAsync(tokenKey);
    }
 
}