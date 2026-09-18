using BankSystem.AuthApi.Data;
using BankSystem.AuthApi.DTOs.LogDto;
using BankSystem.AuthApi.Models;
using BankSystem.AuthApi.Repositories.Interfaces;
using Dapper;
using System.Data;

namespace BankSystem.AuthApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public UserRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<User?> GetByUserNameAsync(string userName)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(
            "usp_GetUserByUserName",
            new { UserName = userName },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<int> CreateUserAsync(int? customerId, string userName, string passwordHash, string role)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(
            "usp_CreateUser",
            new
            {
                CustomerId = customerId,
                UserName = userName,
                PasswordHash = passwordHash,
                Role = role
            },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task DeactivateUserAsync(int userId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(
            "usp_DeactivateUser",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure
        );
    }


    public async Task<IEnumerable<LogResponseDto>> GetLogsAsync(string? tableName, string? action, int? userId, DateTime? fromDate, DateTime? toDate)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<LogResponseDto>(
            "usp_GetLogs",
            new
            {
                TableName = tableName,
                Action = action,
                UserId = userId,
                FromDate = fromDate,
                ToDate = toDate
            },
            commandType: CommandType.StoredProcedure
        );
    }
}