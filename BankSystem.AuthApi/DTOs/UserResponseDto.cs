namespace BankSystem.AuthApi.DTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
