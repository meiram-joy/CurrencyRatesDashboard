namespace CurrencyRates.Application.DTOs;

public class CurrencyRateDto
{
    public string CurrencyCode { get; set; } 
    public string CurrencyName { get; set; } 
    public decimal Rate { get; set; }
    public DateTime Date { get; set; }
}