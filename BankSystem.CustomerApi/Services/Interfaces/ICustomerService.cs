using BankSystem.CustomerApi.DTOs.Customer;
using BankSystem.CustomerApi.DTOs.Individual;
using BankSystem.CustomerApi.DTOs.LegalEntity;

namespace BankSystem.CustomerApi.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<int> CreateIndividualCustomerAsync(CreateIndividualDto dto);
        Task<int> CreateLegalEntityCustomerAsync(CreateLegalEntityDto dto);
        Task<CustomerDetailsDto?> GetCustomerByIdAsync(int id);
        Task<IEnumerable<CustomerDetailsDto>> GetAllCustomersAsync();
        Task<bool> DeactivateCustomerAsync (int id);
    }
}
