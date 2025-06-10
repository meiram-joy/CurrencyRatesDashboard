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

        // Пример JSON от мок-сервера
        // [
        //   { "code": "USD", "rate": 1.0, "name": "US Dollar" },
        //   { "code": "EUR", "rate": 0.92, "name": "Euro" }
        // ]

        var rates = System.Text.Json.JsonSerializer.Deserialize<List<MockCurrencyDto>>(content);

        return rates!
            .Select(dto =>
                CurrencyRateAggregate.Create(
                    CurrencyCode.Create(dto.Code).Value,
                    dto.Name,
                    (decimal)dto.Rate,
                    DateTime.UtcNow
                ).Value
            ).ToList();
    }
    private class MockCurrencyDto
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public double Rate { get; set; }
    }
}