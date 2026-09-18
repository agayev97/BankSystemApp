using BankSystem.AccountApi.Data;
using BankSystem.AccountApi.DTOs;
using BankSystem.AccountApi.Repositories.Interfaces;
using Dapper;
using System.Data;

namespace BankSystem.AccountApi.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DbConnectionFactory _connectionFactory;
        public AccountRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<int> CreateAccountAsync(int customerId, int currencyId, string accountNumber)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(
                "usp_CreateAccount",
                new
                {
                    CustomerId = customerId,
                    CurrencyId = currencyId,
                    AccountNumber = accountNumber
                },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task DeactivateAccountAsync(int accountId)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(
                "usp_DeactivateAccount",
                new { AccountId = accountId },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<AccountResponseDto?> GetAccountByIdAsync(int accountId)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AccountResponseDto>(
                "usp_GetAccount",
                new { AccountId = accountId },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<AccountResponseDto?> GetAccountByNumberAsync(string accountNumber)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AccountResponseDto>(
                "usp_GetAccountByNumber",
                new { AccountNumber = accountNumber },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<AccountBalanceDto?> GetAccountBalanceAsync(int accountId)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AccountBalanceDto>(
                "usp_GetAccountBalance",
                new { AccountId = accountId },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<IEnumerable<AccountResponseDto>> GetCustomerAccountsAsync(int customerId)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<AccountResponseDto>(
                "usp_GetCustomerAccounts",
                new { CustomerId = customerId },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<IEnumerable<CurrencyDto>> GetAllCurrenciesAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT Id, Code, Name, Symbol FROM Currencies ORDER BY Id";
            return await connection.QueryAsync<CurrencyDto>(sql);
        }

    }
}
