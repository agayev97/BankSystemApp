using BankSystem.AccountApi.DTOs;
using BankSystem.AccountApi.Middlewares;
using BankSystem.AccountApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.AccountApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Bankda mövcud olan valyutaların siyahısını gətirir (AZN, USD, EUR)
        /// </summary>
        [HttpGet("currencies")]
        [Authorize(Roles = "Admin,Employee,Customer")]
        public async Task<IActionResult> GetCurrencies()
        {
            var currencies = await _accountService.GetAllCurrenciesAsync();
            return Ok(currencies);
        }

        /// <summary>
        /// Yeni bank hesabı açmaq (Yalnız Admin və İşçilər üçün)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto dto)
        {
            // DTO Validasiyası pozulubsa (Məsələn CustomerId = 0)
            if (!ModelState.IsValid)
            {
                var firstError = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();

                throw new BusinessException(firstError ?? "Məlumatlar yanlış daxil edilib.", ExceptionType.BadRequest);
            }

            // Kod xətası və ya SQL xətası olsa Middleware 500 verib Seriloga yazacaq
            int newAccountId = await _accountService.CreateAccountAsync(dto);
            return Ok(new
            {
                AccountId = newAccountId,
                Message = "Bank hesabı uğurla yaradıldı."
            });
        }

        /// <summary>
        /// Hesab ID-sinə görə hesab məlumatlarını gətirir
        /// </summary>
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetById(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
                throw new BusinessException($"ID-si {id} olan hesab tapılmadı.", ExceptionType.NotFound);

            return Ok(account);
        }

        /// <summary>
        /// Hesab nömrəsinə (IBAN) görə hesabı tapır
        /// </summary>
        [HttpGet("by-number/{accountNumber}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetByAccountNumber(string accountNumber)
        {
            var account = await _accountService.GetAccountByNumberAsync(accountNumber);
            if (account == null)
                throw new BusinessException($"'{accountNumber}' nömrəli hesab tapılmadı.", ExceptionType.NotFound);

            return Ok(account);
        }

        /// <summary>
        /// Müştərinin ID-sinə görə onun bütün bank hesablarını gətirir
        /// </summary>
        [HttpGet("customer/{customerId:int}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetCustomerAccounts(int customerId)
        {
            var accounts = await _accountService.GetCustomerAccountsAsync(customerId);
            return Ok(accounts);
        }

        /// <summary>
        /// Hesabın cari balansını göstərir
        /// </summary>
        [HttpGet("{id:int}/balance")]
        [Authorize(Roles = "Admin,Employee,Customer")]
        public async Task<IActionResult> GetBalance(int id)
        {
            var balance = await _accountService.GetAccountBalanceAsync(id);
            if (balance == null)
                throw new BusinessException("Hesab tapılmadı və ya aktiv deyil.", ExceptionType.NotFound);

            return Ok(balance);
        }

        /// <summary>
        /// Daxil olmuş Müştərinin yalnız ÖZ bank hesablarını gətirir
        /// </summary>
        [HttpGet("my-accounts")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyAccounts()
        {
            var customerIdClaim = User.FindFirst("CustomerId")?.Value;
            if (string.IsNullOrEmpty(customerIdClaim) || !int.TryParse(customerIdClaim, out int customerId))
            {
                throw new BusinessException("Bu istifadəçiyə bağlı müştəri tapılmadı.", ExceptionType.BadRequest);
            }

            var accounts = await _accountService.GetCustomerAccountsAsync(customerId);
            return Ok(accounts);
        }

        /// <summary>
        /// Hesabı bağlamaq / deaktiv etmək (YALNIZ Admin)
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeactivateAccount(int id)
        {
           
            await _accountService.DeactivateAccountAsync(id);
            return Ok(new { Message = "Hesab uğurla bağlandı (deaktiv edildi)." });
        }
    }
}