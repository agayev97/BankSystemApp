using BankSystem.TransactionApi.Data;
using BankSystem.TransactionApi.DTOs;
using BankSystem.TransactionApi.Repositories.Interfaces;
using Dapper;
using System.Data;

namespace BankSystem.TransactionApi.Repositories
{
    public class TransactionRepository  : ITransactionRepository
    {
        private readonly DbConnectionFactory _connectionFactory;
        public TransactionRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task DepositAsync(int accountId, decimal amount, int? userId, string? description)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(
                "usp_Deposit",
                new
                {
                    AccountId = accountId,
                    Amount = amount,
                    UserId = userId,
                    Description = description
                },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task WithdrawAsync(int accountId, decimal amount, int? userId, string? description)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(
                "usp_Withdraw",
                new
                {
                    AccountId = accountId,
                    Amount = amount,
                    UserId = userId,
                    Description = description
                },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task TransferByAccountNumberAsync(int fromAccountId, string toAccountNumber, decimal amount, int? userId, string? description)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(
                "usp_TransferByAccountNumber",
                new
                {
                    FromAccountId = fromAccountId,
                    ToAccountNumber = toAccountNumber,
                    Amount = amount,
                    UserId = userId,
                    Description = description
                },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<IEnumerable<AccountStatementDto>> GetAccountStatementAsync(int accountId, DateTime? fromDate, DateTime? toDate)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<AccountStatementDto>(
                "usp_GetAccountStatement",
                new
                {
                    AccountId = accountId,
                    FromDate = fromDate,
                    ToDate = toDate
                },
                commandType: CommandType.StoredProcedure
            );
        }
        // Təhlükəsizlik yoxlanışı üçün: Bu hesab hansı müştəriyə aiddir?
        public async Task<int?> GetAccountCustomerIdAsync(int accountId)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT CustomerId FROM Accounts WHERE Id = @AccountId";
            return await connection.QueryFirstOrDefaultAsync<int?>(sql, new { AccountId = accountId });
        }

    }
}
