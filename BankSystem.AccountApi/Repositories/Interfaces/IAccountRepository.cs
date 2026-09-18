using BankSystem.AccountApi.DTOs;

namespace BankSystem.AccountApi.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<int> CreateAccountAsync(int customerId, int currencyId, string accountNumber);
        Task DeactivateAccountAsync(int accountId);
        Task<AccountResponseDto?> GetAccountByIdAsync(int accountId);
        Task<AccountResponseDto?> GetAccountByNumberAsync(string accountNumber);
        Task<AccountBalanceDto?> GetAccountBalanceAsync(int accountId);
        Task<IEnumerable<AccountResponseDto>> GetCustomerAccountsAsync(int customerId);
        Task<IEnumerable<CurrencyDto>> GetAllCurrenciesAsync();
    }
}
