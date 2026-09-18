using System.ComponentModel.DataAnnotations;

namespace BankSystem.AccountApi.DTOs
{
    public class CreateAccountDto
    {
        [Required(ErrorMessage = "CustomerId mütləq daxil edilməlidir.")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "CurrencyId mütləq seçilməlidir.")]
        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "AccountNumber mütləqdir.")]
        public string AccountNumber { get; set; } = string.Empty;
    }
}
