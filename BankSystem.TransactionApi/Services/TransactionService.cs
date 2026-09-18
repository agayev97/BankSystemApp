using BankSystem.TransactionApi.DTOs;
using BankSystem.TransactionApi.Repositories.Interfaces;
using BankSystem.TransactionApi.Services.Interfaces;

namespace BankSystem.TransactionApi.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task DepositAsync(DepositDto dto, int? currentUserId)
        {
            await _transactionRepository.DepositAsync(dto.AccountId, dto.Amount, currentUserId, dto.Description);
        }

        public async Task WithdrawAsync(WithdrawDto dto, int? currentUserId)
        {
            await _transactionRepository.WithdrawAsync(dto.AccountId, dto.Amount, currentUserId, dto.Description);
        }

        public async Task TransferAsync(TransferByAccountNumberDto dto, int? currentUserId, int? currentCustomerId, string userRole)
        {
            // KRİTİK TƏHLÜKƏSİZLİK: Müştəri yalnız öz hesabından pul köçürə bilər!
            if (userRole == "Customer")
            {
                var accountCustomerId = await _transactionRepository.GetAccountCustomerIdAsync(dto.FromAccountId);
                if (accountCustomerId == null || accountCustomerId != currentCustomerId)
                {
                    throw new UnauthorizedAccessException("Qadağandır! Siz yalnız öz hesabınızdan köçürmə edə bilərsiniz.");
                }
            }
            string cleanToAccountNumber = dto.ToAccountNumber.Trim().ToUpper();
            await _transactionRepository.TransferByAccountNumberAsync(
                dto.FromAccountId,
                cleanToAccountNumber,
                dto.Amount,
                currentUserId,
                dto.Description
            );
        }

        public async Task<IEnumerable<AccountStatementDto>> GetStatementAsync(int accountId, DateTime? fromDate, DateTime? toDate, int? currentCustomerId, string userRole)
        {
            // Müştəri yalnız öz hesabının çıxarışına baxa bilər!
            if (userRole == "Customer")
            {
                var accountCustomerId = await _transactionRepository.GetAccountCustomerIdAsync(accountId);
                if (accountCustomerId == null || accountCustomerId != currentCustomerId)
                {
                    throw new UnauthorizedAccessException("Qadağandır! Siz yalnız öz hesabınızın çıxarışına baxa bilərsiniz.");
                }
            }
            return await _transactionRepository.GetAccountStatementAsync(accountId, fromDate, toDate);
        }

    }
}
