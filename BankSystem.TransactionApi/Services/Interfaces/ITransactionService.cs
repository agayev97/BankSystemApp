using BankSystem.TransactionApi.DTOs;

namespace BankSystem.TransactionApi.Services.Interfaces
{
    public interface ITransactionService
    {
        Task DepositAsync(DepositDto dto, int? currentUserId);
        Task WithdrawAsync(WithdrawDto dto, int? currentUserId);
        Task TransferAsync(TransferByAccountNumberDto dto, int? currentUserId, int? currentCustomerId, string userRole);
        Task<IEnumerable<AccountStatementDto>> GetStatementAsync(int accountId, DateTime? fromDate, DateTime? toDate, int? currentCustomerId, string userRole);
    }
}
