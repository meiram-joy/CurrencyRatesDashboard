using Microsoft.JSInterop;

namespace CurrencyRatesDashboard.BlazoreUIss.Services.Auth;

public class CookieService
{
    private readonly IJSRuntime _jsRuntime;
    
    public CookieService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<string> GetCookieAsync(string key)
    {
        return await _jsRuntime.InvokeAsync<string>("getCookie",key);
    }
    
    public async Task RemoveCookieAsync(string key)
    {
         await _jsRuntime.InvokeVoidAsync("deleteCookie", key);
    }
    
    public async Task SetCookieAsync(string key, string value, int days)
    {
        await _jsRuntime.InvokeVoidAsync("setCookie", key, value, days);
    }
}