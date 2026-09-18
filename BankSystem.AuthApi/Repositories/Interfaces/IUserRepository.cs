using BankSystem.AuthApi.DTOs.LogDto;
using BankSystem.AuthApi.Models;

namespace BankSystem.AuthApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUserNameAsync(string userName);
        Task<int> CreateUserAsync(int? customerId, string userName, string passwordHash, string role);
        Task DeactivateUserAsync(int userId);

        Task<IEnumerable<LogResponseDto>> GetLogsAsync(string? tableName, string? action, int? userId, DateTime? fromDate, DateTime? toDate);
    }
}
