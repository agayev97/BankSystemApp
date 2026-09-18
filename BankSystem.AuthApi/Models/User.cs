namespace BankSystem.AuthApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; } // Null ola bilər (Admin və İşçilər üçün)
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // 'Admin', 'Employee', 'Customer'
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
