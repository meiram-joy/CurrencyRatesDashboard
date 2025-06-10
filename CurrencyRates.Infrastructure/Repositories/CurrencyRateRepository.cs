using System.Data;
using CurrencyRates.Domain.Currency.Aggregates;
using CurrencyRates.Domain.Currency.Entities;
using CurrencyRates.Domain.Currency.Interfaces;
using CurrencyRates.Domain.Currency.ValueObjects;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CurrencyRates.Infrastructure.Repositories;

public class CurrencyRateRepository : ICurrencyRateRepository
{
    private readonly IDbConnection _connection;
    
    public CurrencyRateRepository(IConfiguration configuration)
    {
        _connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }
    public async Task<IReadOnlyList<CurrencyRateAggregate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT Code, Name, Rate, RetrievedAt FROM CurrencyRates";

        var result = await _connection.QueryAsync<CurrencyRateDto>(sql);

        return result
            .Select(r =>
                CurrencyRateAggregate.Create(
                    CurrencyCode.Create(r.Code).Value,
                    r.Name,
                    r.Rate,
                    r.RetrievedAt
                ).Value
            ).ToList();
    }

    public async Task<CurrencyRateAggregate?> GetByCodeAsync(CurrencyCode currencyCode, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT Code, Name, Rate, RetrievedAt FROM CurrencyRates WHERE Code = @Code";

        var dto = await _connection.QuerySingleOrDefaultAsync<CurrencyRateDto>(sql, new { Code = currencyCode.Code });

        if (dto == null)
            return null;

        return CurrencyRateAggregate.Create(
            CurrencyCode.Create(dto.Code).Value,
            dto.Name,
            dto.Rate,
            dto.RetrievedAt
        ).Value;
    }

    public async Task AddAsync(CurrencyRateAggregate rate, CancellationToken cancellationToken = default)
    {
        const string sql = @"INSERT INTO CurrencyRates (Code, Name, Rate, RetrievedAt) 
                             VALUES (@Code, @Name, @Rate, @RetrievedAt)";

        await _connection.ExecuteAsync(sql, new
        {
            Code = rate.CurrencyCode.Code,
            rate.Name,
            rate.Rate,
            rate.UpdatedAt
        });
    }

    public async Task UpdateAsync(CurrencyRateAggregate rate, CancellationToken cancellationToken = default)
    {
        const string sql = @"UPDATE CurrencyRates 
                             SET Name = @Name, Rate = @Rate, RetrievedAt = @RetrievedAt 
                             WHERE Code = @Code";

        await _connection.ExecuteAsync(sql, new
        {
            Code = rate.CurrencyCode.Code,
            rate.Name,
            rate.Rate,
            rate.UpdatedAt
        });
    }

    private record CurrencyRateDto(string Code, string Name, decimal Rate, DateTime RetrievedAt);
}