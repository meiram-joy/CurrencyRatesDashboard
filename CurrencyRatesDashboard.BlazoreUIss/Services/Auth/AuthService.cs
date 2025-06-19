using CurrencyRatesDashboard.BlazoreUIss.Model.Auth;
using Microsoft.AspNetCore.Components;

namespace CurrencyRatesDashboard.BlazoreUIss.Services.Auth;

public class AuthService
{
    private readonly AccessTokenService _accessTokenService;
    private readonly NavigationManager _nav;

    private readonly HttpClient _http;

    public AuthService(AccessTokenService accessTokenService,NavigationManager nav, HttpClient http)
    {
        _accessTokenService = accessTokenService;
        _nav = nav;
        _http = http;
    }
    public async Task<bool> LoginAsync(string email, string password)
    {
        var status = await _http.PostAsJsonAsync("http://localhost:5280/api/auth/login", new
        {
            Email = email,
            Password = password
        });
        
        if (status.IsSuccessStatusCode)
        {
            var token = await status.Content.ReadAsStreamAsync();
            
            var result = await status.Content.ReadFromJsonAsync<LoginResponse>();
            
            await _accessTokenService.SetTokenAsync(result.AccessToken);
            
            return true;
        }
        else
        {
            return false;
        }

    }

    public async Task<bool> RegisterAsync(string email, string password,string role)
    {
        var response = await _http.PostAsJsonAsync("http://localhost:5280/api/auth/register", new
        {
            Email = email,
            Password = password,
            Role = role
        });
        
        
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> LogoutAsync(string refreshToken)
    {
        var response = await _http.PostAsJsonAsync("api/auth/logout", new
        {
            RefreshToken = refreshToken
        });

        return response.IsSuccessStatusCode;
    }

    public async Task<RefreshResponse?> RefreshAsync(string refreshToken)
    {
        var response = await _http.PostAsJsonAsync("api/auth/refresh", new
        {
            RefreshToken = refreshToken
        });

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<RefreshResponse>();
            return result;
        }

        return null;
    }
    
}