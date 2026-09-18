using BankSystem.TransactionApi.DTOs;

namespace BankSystem.TransactionApi.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task DepositAsync(int accountId, decimal amount, int? userId, string? description);
        Task WithdrawAsync(int accountId, decimal amount, int? userId, string? description);
        Task TransferByAccountNumberAsync(int fromAccountId, string toAccountNumber, decimal amount, int? userId, string? description);
        Task<IEnumerable<AccountStatementDto>> GetAccountStatementAsync(int accountId, DateTime? fromDate, DateTime? toDate);
        Task<int?> GetAccountCustomerIdAsync(int accountId);
    }
}
