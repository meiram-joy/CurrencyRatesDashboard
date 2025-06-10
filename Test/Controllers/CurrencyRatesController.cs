using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using CurrencyRates.Application.Interfaces;
using CurrencyRates.Application.DTOs;
using CurrencyRates.Application.Queries;
using MediatR;

namespace CurrencyRates.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurrencyRatesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IExportService _exportService;
    
    public CurrencyRatesController(IMediator mediator,IExportService exportService)
    {
        _mediator = mediator;
        _exportService = exportService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CurrencyRateDto>>> GetAll()
    {
        var query = new GetCurrencyRatesQuery();
        var rates = await _mediator.Send(query);
        return Ok(rates);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshRates()
    {
        var query = new GetCurrencyRatesQuery();
        await _mediator.Send(query);
        return Ok();
    }
    [HttpGet("export")]
    public async Task<IActionResult> ExportRates()
    {
        var query = new GetCurrencyRatesQuery();
        var rates = await _mediator.Send(query);
        var fileName = $"CurrencyRates_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Data");
    
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        var bytes = stream.ToArray();
    
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }
    
}