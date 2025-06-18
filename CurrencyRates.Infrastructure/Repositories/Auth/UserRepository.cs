using CurrencyRates.Domain.Currency.Aggregates.Auth;
using CurrencyRates.Domain.Currency.Interfaces.Auth;
using CurrencyRates.Domain.Currency.ValueObjects.Auth;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace CurrencyRates.Infrastructure.Repositories.Auth;

public class UserRepository : IUserRepository
{
    private readonly IConfiguration _configuration;

    public UserRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        EnsureTableCreated(_configuration);
    }
    
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync(cancellationToken);
        
        const string sql = @"SELECT Id, Email, PasswordHash, Role FROM Users WHERE Id = @Id";
        
        var userDto = await connection.QuerySingleOrDefaultAsync<UserDto>(sql, new { Id = id });
        
        if (userDto == null)
            return null;
        
        return MapToUser(userDto);
    }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        using var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync(cancellationToken);

        const string sql = @"SELECT Id, Email, PasswordHash, Role FROM Users WHERE LOWER(Email) = LOWER(@Email)";

        var userDto = await connection.QuerySingleOrDefaultAsync<UserDto>(sql, new { Email = email.Value });
        
        return MapToUser(userDto);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        using var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync(cancellationToken);

        const string sql = @"INSERT INTO Users (Id, Email, PasswordHash, Role)
                             VALUES (@Id, @Email, @PasswordHash, @Role)";

        await connection.ExecuteAsync(sql, new
        {
            Id = user.ID,
            Email = user.Email.Value,
            PasswordHash = user.PasswordHash.Hash,
            Role = user.Role.Name
        });
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        using var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync(cancellationToken);

        const string sql = @"UPDATE Users 
                             SET Email = @Email,
                                 PasswordHash = @PasswordHash,
                                 Role = @Role
                             WHERE Id = @Id";

        await connection.ExecuteAsync(sql, new
        {
            Id = user.ID,
            Email = user.Email.Value,
            PasswordHash = user.PasswordHash.Hash,
            Role = user.Role.Name
        });
    }
    
    private void EnsureTableCreated(IConfiguration _configuration)
    {
        using var connection = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection"));
        connection.Open();
        var createTableSql = @"
        CREATE TABLE IF NOT EXISTS Users (
            Id TEXT PRIMARY KEY,
            Email VARCHAR(256) NOT NULL UNIQUE,
            PasswordHash VARCHAR(512) NOT NULL,
            Role VARCHAR(50) NOT NULL
        );
    ";
        using var cmd = connection.CreateCommand();
        cmd.CommandText = createTableSql;
        cmd.ExecuteNonQuery();
    }
    
    private static User? MapToUser(UserDto? dto)
    {
        if (dto is null)
            return null;
        

        var result = User.Create(dto.Id,dto.Email, dto.PasswordHash, dto.Role);
        return result.IsSuccess ? result.Value : null;
    }
    
    private class UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string Role { get; set; } = default!;
    }
}