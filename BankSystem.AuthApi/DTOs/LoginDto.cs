using System.ComponentModel.DataAnnotations;

namespace BankSystem.AuthApi.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "İstifadəçi adı daxil edilməlidir.")]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Şifrə daxil edilməlidir.")]
        public string Password { get; set; } = string.Empty;
    }
}
