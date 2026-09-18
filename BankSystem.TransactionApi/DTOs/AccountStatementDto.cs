namespace BankSystem.TransactionApi.DTOs
{
    public class AccountStatementDto
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public string Direction { get; set; } = string.Empty; // IN və ya OUT
        public decimal Amount { get; set; }
        public decimal CurrencyRate { get; set; }
        public string? Description { get; set; }
        public int FromAccountId { get; set; }
        public int? ToAccountId { get; set; }
    }
}
