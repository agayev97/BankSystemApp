
using BankSystem.AccountApi.DTOs;
using BankSystem.AccountApi.Repositories.Interfaces;
using BankSystem.AccountApi.Services.Interfaces;

namespace BankSystem.AccountApi.Services
{
    public class AccountService : IAccountService
    {

        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<int> CreateAccountAsync(CreateAccountDto dto)
        {
            // AccountNumber-in böyük hərflərlə yazılmasını və kənar boşluqların silinməsini təmin edirik
            string cleanAccountNumber = dto.AccountNumber.Trim().ToUpper();
            return await _accountRepository.CreateAccountAsync(dto.CustomerId, dto.CurrencyId, cleanAccountNumber);
        }

        public async Task DeactivateAccountAsync(int accountId)
        {
            await _accountRepository.DeactivateAccountAsync(accountId);
        }

        public async Task<AccountResponseDto?> GetAccountByIdAsync(int accountId)
        {
            return await _accountRepository.GetAccountByIdAsync(accountId);
        }

        public async Task<AccountResponseDto?> GetAccountByNumberAsync(string accountNumber)
        {
            return await _accountRepository.GetAccountByNumberAsync(accountNumber.Trim().ToUpper());
        }

        public async Task<AccountBalanceDto?> GetAccountBalanceAsync(int accountId)
        {
            return await _accountRepository.GetAccountBalanceAsync(accountId);
        }

        public async Task<IEnumerable<AccountResponseDto>> GetCustomerAccountsAsync(int customerId)
        {
            return await _accountRepository.GetCustomerAccountsAsync(customerId);
        }

        public async Task<IEnumerable<CurrencyDto>> GetAllCurrenciesAsync()
        {
            return await _accountRepository.GetAllCurrenciesAsync();
        }

    }
}
