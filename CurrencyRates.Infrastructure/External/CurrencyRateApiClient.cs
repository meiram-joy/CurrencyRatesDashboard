using CurrencyRates.Application.Interfaces;
using CurrencyRates.Domain.Currency.Aggregates;
using CurrencyRates.Domain.Currency.ValueObjects;

namespace CurrencyRates.Infrastructure.External;

public class CurrencyRateApiClient : ICurrencyRateApiClient
{
    private readonly HttpClient _httpClient;

    public CurrencyRateApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<IReadOnlyCollection<CurrencyRateAggregate>> GetLatestRatesAsync()
    {
        var response = await _httpClient.GetAsync("https://8b29c522-a109-4b79-996f-a2120dbc9d5b.mock.pstmn.io/api/testCurrencyRate");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        var rates = System.Text.Json.JsonSerializer.Deserialize<List<MockCurrencyDto>>(content);

        var validRates = rates!
            .Where(dto => !string.IsNullOrWhiteSpace(dto.Code))
            .ToList();

        return validRates
            .Select(dto =>
                CurrencyRateAggregate.Create(
                    CurrencyCode.Create(dto.Code).Value,
                    dto.Name,
                    dto.Rate,
                    dto.RetrievedAt
                ).Value
            ).ToList();
    }
    private class MockCurrencyDto
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public double Rate { get; set; }
        public DateTime RetrievedAt { get; set; } = DateTime.UtcNow;
    }
}