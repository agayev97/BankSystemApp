using System.ComponentModel.DataAnnotations;

namespace BankSystem.AuthApi.DTOs
{
    public class RegisterDto
    {
        public int? CustomerId { get; set; }
        [Required(ErrorMessage = "İstifadəçi adı mütləqdir.")]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Şifrə mütləqdir.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifrə ən azı 6 simvol olmalıdır.")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Rol mütləqdir.")]
        [RegularExpression("^(Admin|Employee|Customer)$", ErrorMessage = "Rol yalnız Admin, Employee və ya Customer ola bilər.")]
        public string Role { get; set; } = string.Empty;
    }
}
