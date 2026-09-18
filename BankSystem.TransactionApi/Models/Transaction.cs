namespace BankSystem.TransactionApi.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int? ToAccountId { get; set; }
        public string? ToAccountNumber { get; set; }
        public int? UserId { get; set; }
        public string TransactionType { get; set; } = string.Empty; // Deposit, Withdraw, Transfer
        public decimal Amount { get; set; }
        public decimal CurrencyRate { get; set; }
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; }

    }
}
