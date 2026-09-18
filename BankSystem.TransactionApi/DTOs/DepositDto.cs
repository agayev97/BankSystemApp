using System.ComponentModel.DataAnnotations;

namespace BankSystem.TransactionApi.DTOs
{
    public class DepositDto
    {
        [Required(ErrorMessage = "AccountId mütləqdir.")]
        public int AccountId { get; set; }
        [Required(ErrorMessage = "Məbləğ daxil edilməlidir.")]
        [Range(0.01, 10000000, ErrorMessage = "Məbləğ 0-dan böyük olmalıdır.")]
        public decimal Amount { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
    }
}
