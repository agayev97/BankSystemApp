using BankSystem.AccountApi.DTOs;

namespace BankSystem.AccountApi.Services.Interfaces
{
    public interface IAccountService
    {
        Task<int> CreateAccountAsync(CreateAccountDto dto);
        Task DeactivateAccountAsync(int accountId);
        Task<AccountResponseDto?> GetAccountByIdAsync(int accountId);
        Task<AccountResponseDto?> GetAccountByNumberAsync(string accountNumber);
        Task<AccountBalanceDto?> GetAccountBalanceAsync(int accountId);
        Task<IEnumerable<AccountResponseDto>> GetCustomerAccountsAsync(int customerId);
        Task<IEnumerable<CurrencyDto>> GetAllCurrenciesAsync();
    }
}
