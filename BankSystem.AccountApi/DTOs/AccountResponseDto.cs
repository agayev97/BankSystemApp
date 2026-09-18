namespace BankSystem.AccountApi.DTOs
{
    public class AccountResponseDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int CurrencyId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public DateTime OpenDate { get; set; }
        public bool IsActive { get; set; }

        // Currencies cədvəlindən Join ilə gələn sahələr:
        public string CurrencyCode { get; set; } = string.Empty;
        public string CurrencyName { get; set; } = string.Empty;
    }
}
