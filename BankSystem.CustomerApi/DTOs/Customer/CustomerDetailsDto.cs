using BankSystem.CustomerApi.DTOs.Individual;
using BankSystem.CustomerApi.DTOs.LegalEntity;

namespace BankSystem.CustomerApi.DTOs.Customer
{
    public class CustomerDetailsDto
    {
        public int CustomerId { get; set; }
        public string CustomerType { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

        public IndividualResponseDto? Individual { get; set; }
        public LegalEntityResponseDto? LegalEntity { get; set; }
    }
}
