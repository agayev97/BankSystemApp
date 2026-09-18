namespace BankSystem.AccountApi.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; // AZN, USD, EUR
        public string Name { get; set; } = string.Empty; // Azərbaycan Manatı, ABŞ Dolları
        public string? Symbol { get; set; }              // ₼, $, €
    }
}
