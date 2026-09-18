using BankSystem.TransactionApi.DTOs;
using BankSystem.TransactionApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankSystem.TransactionApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController  : ControllerBase
    {

        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        /// <summary>
        /// Hesaba nağd pul yatırmaq (Mədaxil - Yalnız Filial İşçisi və Admin)
        /// </summary>
        [HttpPost("deposit")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Deposit([FromBody] DepositDto dto)
        {
            int? userId = GetCurrentUserId();
            await _transactionService.DepositAsync(dto, userId);
            return Ok(new { Message = $"{dto.Amount} AZN məbləğində mədaxil uğurla tamamlandı." });
        }
        /// <summary>
        /// Hesabdan nağd pul çıxarmaq (Məxaric - Yalnız Filial İşçisi və Admin)
        /// </summary>
        [HttpPost("withdraw")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawDto dto)
        {
            int? userId = GetCurrentUserId();
            await _transactionService.WithdrawAsync(dto, userId);
            return Ok(new { Message = $"{dto.Amount} AZN məbləğində məxaric uğurla tamamlandı." });
        }
        /// <summary>
        /// Hesab nömrəsi ilə (IBAN) başqa hesaba pul köçürmək (Həm Müştəri, həm İşçi)
        /// </summary>
        [HttpPost("transfer")]
        [Authorize(Roles = "Admin,Employee,Customer")]
        public async Task<IActionResult> Transfer([FromBody] TransferByAccountNumberDto dto)
        {
            int? userId = GetCurrentUserId();
            int? customerId = GetCurrentCustomerId();
            string role = GetCurrentUserRole();
            await _transactionService.TransferAsync(dto, userId, customerId, role);
            return Ok(new { Message = "Pul köçürməsi uğurla icra olundu." });
        }
        /// <summary>
        /// Hesabın çıxarışı / Əməliyyat tarixçəsi (Həm Müştəri, həm İşçi)
        /// </summary>
        [HttpGet("statement/{accountId:int}")]
        [Authorize(Roles = "Admin,Employee,Customer")]
        public async Task<IActionResult> GetStatement(int accountId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {

            int? customerId = GetCurrentCustomerId();
            string role = GetCurrentUserRole();
            var statement = await _transactionService.GetStatementAsync(accountId, fromDate, toDate, customerId, role);
            return Ok(statement);
          
        }
        // Köməkçi metodlar: Tokendən məlumatları oxuyur
        private int? GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out int id) ? id : null;
        }
        private int? GetCurrentCustomerId()
        {
            var claim = User.FindFirst("CustomerId")?.Value;
            return int.TryParse(claim, out int id) ? id : null;
        }
        private string GetCurrentUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }

    }
}
