using System.ComponentModel.DataAnnotations;

namespace BankSystem.TransactionApi.DTOs
{
    public class TransferByAccountNumberDto
    {
        [Required(ErrorMessage = "Göndərən hesab mütləqdir.")]
        public int FromAccountId { get; set; }
        [Required(ErrorMessage = "Qəbul edən hesab nömrəsi mütləqdir.")]
        [StringLength(34, MinimumLength = 10)]
        public string ToAccountNumber { get; set; } = string.Empty;
        [Required(ErrorMessage = "Məbləğ daxil edilməlidir.")]
        [Range(0.01, 10000000, ErrorMessage = "Məbləğ 0-dan böyük olmalıdır.")]
        public decimal Amount { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
    }
}
