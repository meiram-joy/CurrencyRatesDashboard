using CurrencyRates.Domain.Currency.Interfaces.Auth;
using CurrencyRates.Domain.Currency.ValueObjects.Auth;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace CurrencyRates.Infrastructure.Repositories.Auth;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IConfiguration _configuration;
    public RefreshTokenRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        EnsureTableCreated(_configuration);
    }
    public async Task SaveAsync(Guid userId, RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        using var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync(cancellationToken);

        const string sql = @"INSERT INTO RefreshTokens (UserId, Token, Expires)
                             VALUES (@UserId, @Token, @Expires)";

        await connection.ExecuteAsync(sql, new
        {
            UserId = userId,
            Token = refreshToken.Token,
            Expires = refreshToken.Expires
        });
    }

    public async Task<bool> ValidateAsync(Guid userId, RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        using var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync(cancellationToken);

        const string sql = @"SELECT COUNT(1)
                             FROM RefreshTokens
                             WHERE UserId = @UserId AND Token = @Token AND Expires > @Now";

        var count = await connection.ExecuteScalarAsync<int>(sql, new
        {
            UserId = userId,
            Token = refreshToken.Token,
            Now = DateTime.UtcNow
        });

        return count > 0;
    }

    public async Task InvalidateAsync(Guid userId, RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        using var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync(cancellationToken);

        const string sql = @"DELETE FROM RefreshTokens WHERE UserId = @UserId AND Token = @Token";

        await connection.ExecuteAsync(sql, new
        {
            UserId = userId,
            Token = refreshToken.Token
        });
    }

    public async Task<RefreshToken?> GetAsync(Guid userId, string token, CancellationToken cancellationToken = default)
    {
        using var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync(cancellationToken);

        const string sql = @"SELECT Token, Expires
                             FROM RefreshTokens
                             WHERE UserId = @UserId AND Token = @Token";

        var dto = await connection.QuerySingleOrDefaultAsync<RefreshTokenDto>(sql, new
        {
            UserId = userId,
            Token = token
        });

        return MapToRefreshToken(dto);
    }
    private static RefreshToken? MapToRefreshToken(RefreshTokenDto? dto)
    {
        if (dto == null)
            return null;

        return RefreshToken.Create(dto.Token, dto.Expires);
    }
    
    private void EnsureTableCreated(IConfiguration _configuration)
    {
        using var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection"));
        connection.Open();
        var createTableSql = @"
            CREATE TABLE IF NOT EXISTS RefreshTokens (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                UserId VARCHAR(36) NOT NULL,
                Token VARCHAR(512) NOT NULL UNIQUE,
                Expires VARCHAR(50) NOT NULL,
                IsInvalidated INTEGER NOT NULL DEFAULT 0,
                FOREIGN KEY(UserId) REFERENCES Users(Id)
            );
        ";
        using var cmd = connection.CreateCommand();
        cmd.CommandText = createTableSql;
        cmd.ExecuteNonQuery();
    }
    private class RefreshTokenDto
    {
        public string Token { get; set; } = default!;
        public DateTime Expires { get; set; }
    }
}