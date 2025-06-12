using System.Data;
using CurrencyRates.Domain.Currency.Aggregates;
using CurrencyRates.Domain.Currency.Interfaces;
using CurrencyRates.Domain.Currency.ValueObjects;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace CurrencyRates.Infrastructure.Repositories;

public class CurrencyRateRepository : ICurrencyRateRepository
{
    private readonly IConfiguration _configuration;
    
    public CurrencyRateRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        EnsureTableCreated(_configuration);
    }
    public async Task<IReadOnlyList<CurrencyRateAggregate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync(cancellationToken);
        
        const string sql = @"SELECT Code, Name, Rate, RetrievedAt FROM CurrencyRates";
        
        var result = await connection.QueryAsync<CurrencyRateDto>(sql);

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
        using var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync(cancellationToken);
        
        const string sql = @"SELECT Code, Name, Rate, RetrievedAt FROM CurrencyRates WHERE Code = @Code";

        var dto = await connection.QuerySingleOrDefaultAsync<CurrencyRateDto>(sql, new { Code = currencyCode.Code });

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
        using var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync(cancellationToken);
        
        const string sql = @"INSERT INTO CurrencyRates (Code, Name, Rate, RetrievedAt) 
                             VALUES (@Code, @Name, @Rate, @RetrievedAt)";

        try
        {
            await connection.ExecuteAsync(sql, new
            {
                Code = rate.CurrencyCode.Code,
                Name = rate.Name,
                Rate = rate.Rate,
                RetrievedAt = rate.RetrievedAt
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при вставке: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateAsync(CurrencyRateAggregate rate, CancellationToken cancellationToken = default)
    {
        using var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync(cancellationToken);
        
        const string sql = @"UPDATE CurrencyRates 
                             SET Name = @Name, Rate = @Rate, RetrievedAt = @RetrievedAt 
                             WHERE Code = @Code";

        await connection.ExecuteAsync(sql, new
        {
            Code = rate.CurrencyCode.Code,
            Name = rate.Name,
            Rate = rate.Rate,
            RetrievedAt = rate.RetrievedAt
        });
    }

    public class CurrencyRateDto
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public double Rate { get; set; }
        public DateTime RetrievedAt { get; set; }
    }
    
    private void EnsureTableCreated(IConfiguration _configuration)
    {
        using var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection"));
        connection.Open();
        var createTableSql = @"
            CREATE TABLE IF NOT EXISTS CurrencyRates (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Code NVARCHAR(50) NOT NULL,
                Name NVARCHAR(20) NOT NULL,
                Rate REAL NOT NULL,
                RetrievedAt DATETIME NOT NULL
            );
        ";
        using var cmd = connection.CreateCommand();
        cmd.CommandText = createTableSql;
        cmd.ExecuteNonQuery();
    }
}