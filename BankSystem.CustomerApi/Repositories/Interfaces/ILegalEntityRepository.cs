using BankSystem.CustomerApi.Models;

namespace BankSystem.CustomerApi.Repositories.Interfaces
{
    public interface ILegalEntityRepository
    {
        Task<LegalEntity?> GetByCustomerIdAsync(int customerId);
        Task<int> CreateAsync(LegalEntity legalEntity);
        Task<bool> UpdateAsync(LegalEntity legalEntity);

    }
}
