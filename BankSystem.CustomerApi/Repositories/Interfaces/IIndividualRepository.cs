using BankSystem.CustomerApi.Models;

namespace BankSystem.CustomerApi.Repositories.Interfaces
{
    public interface IIndividualRepository
    {
        Task<Individual?> GetByCustomerIdAsync(int customerId);
        Task<int> CreateAsync(Individual individual);
        Task<bool> UpdateAsync(Individual individual);

    }
}
