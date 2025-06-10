using Microsoft.AspNetCore.Mvc;
using CurrencyRates.Application.Interfaces;
using CurrencyRates.Application.DTOs;

namespace CurrencyRates.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurrencyRatesController : ControllerBase
{
    private readonly ICurrencyRateService _currencyRateService;
    
    public CurrencyRatesController(ICurrencyRateService currencyRateService)
    {
        _currencyRateService = currencyRateService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CurrencyRateDto>>> GetAll()
    {
        var rates = await _currencyRateService.GetAllAsync();
        return Ok(rates);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshRates()
    {
        await _currencyRateService.RefreshRatesAsync();
        return Ok();
    }
}