namespace CurrencyRatesDashboard.BlazoreUIss.Model.Auth;

public class UserInfoDto
{
    public string Id { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Role { get; set; } = "User";
}