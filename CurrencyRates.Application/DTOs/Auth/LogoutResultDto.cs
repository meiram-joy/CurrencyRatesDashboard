namespace CurrencyRates.Application.DTOs.Auth;

public class LogoutResultDto
{
    public bool Success { get; set; }
    public string Error { get; set; }

    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    public LogoutResultDto() { }

    public LogoutResultDto(bool success, string error)
    {
        Success = success;
        Error = error;
    }
}