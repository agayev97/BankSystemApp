namespace BankSystem.AccountApi.Models
{
    public class Account
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int CurrencyId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public DateTime OpenDate { get; set; }
        public bool IsActive { get; set; }
    }
}
