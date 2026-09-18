using BankSystem.AuthApi.DTOs;
using BankSystem.AuthApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.AuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        /// <summary>
        /// İstifadəçinin sistemə daxil olması və JWT Token əldə etməsi
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var response = await _authService.LoginAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // İstifadəçi tapılmadıqda və ya şifrə yanlış olduqda
                return Unauthorized(new { message = ex.Message });
            }
        }
        /// <summary>
        /// Yeni istifadəçi qeydiyyatı (Admin, İşçi və ya Müştəri)
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                int newUserId = await _authService.RegisterAsync(dto);
                return Ok(new
                {
                    message = "İstifadəçi uğurla yaradıldı.",
                    userId = newUserId
                });
            }
            catch (Exception ex)
            {
                // SQL Stored Procedure-dan THROW ilə qayıdan xətalar (məs: Bu username artıq var)
                return BadRequest(new { message = ex.Message });
            }
        }
        /// <summary>
        /// İstifadəçi hesabının dondurulması / deaktiv edilməsi
        /// </summary>
        [HttpPut("deactivate/{id:int}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            try
            {
                await _authService.DeactivateAsync(id);
                return Ok(new { message = "İstifadəçi hesabı uğurla deaktiv edildi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
