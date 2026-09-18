using BankSystem.AuthApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.AuthApi.Controllers
{
    [Authorize(Roles = "Admin")] // Adi işçi və ya müştəri loqları görə bilməz!
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public LogController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs(
            [FromQuery] string? tableName,
            [FromQuery] string? action,
            [FromQuery] int? userId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var logs = await _userRepository.GetLogsAsync(tableName, action, userId, fromDate, toDate);
            return Ok(logs);
        }



    }
}
