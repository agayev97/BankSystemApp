using BankSystem.CustomerApi.Models;

namespace BankSystem.CustomerApi.Repositories.Interfaces
{
    public interface IStakeholderRepository
    {
        Task<IEnumerable<Stakeholder>> GetByLegalEntityIdAsync(int legalEntityId);
        Task<int> CreateAsync(Stakeholder stakeholder);
        Task<bool> UpdateAsync(Stakeholder stakeholder);
        Task<bool> DeleteAsync(int id);
    }
}
