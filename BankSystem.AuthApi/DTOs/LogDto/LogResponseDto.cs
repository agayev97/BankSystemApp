namespace BankSystem.AuthApi.DTOs.LogDto
{
    public class LogResponseDto
    {
        public long Id { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string TableName { get; set; } = string.Empty;
        public int? RecordId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? Details { get; set; }
        public DateTime LogDate { get; set; }
    }
}
