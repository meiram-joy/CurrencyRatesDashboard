﻿namespace CurrencyRates.Application.DTOs.Auth;

public class LogoutRequestDto
{
    public string RefreshToken { get; set; } = default!;
}