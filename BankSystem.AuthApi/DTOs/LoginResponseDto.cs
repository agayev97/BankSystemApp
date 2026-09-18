namespace BankSystem.AuthApi.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int? CustomerId { get; set; }
        public DateTime Expiration { get; set; }
    }
}
