using BankSystem.CustomerApi.DTOs.Stakeholder;

namespace BankSystem.CustomerApi.Services.Interfaces
{
    public interface IStakeholderService
    {
        Task<IEnumerable<StakeholderResponseDto>> GetByLegalEntityIdAsync(int legalEntityId);
        Task<int> CreateAsync(CreateStakeholderDto dto);
        Task<bool> UpdateAsync(int id, UpdateStakeholderDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
