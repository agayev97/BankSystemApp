using BankSystem.AuthApi.DTOs;

namespace BankSystem.AuthApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto dto);
        Task<int> RegisterAsync(RegisterDto dto);
        Task DeactivateAsync(int userId);
    }
}
