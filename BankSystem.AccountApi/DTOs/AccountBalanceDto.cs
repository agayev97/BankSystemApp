namespace BankSystem.AccountApi.DTOs
{
    public class AccountBalanceDto
    {
        public int AccountId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public int CurrencyId { get; set; }
    }
}
