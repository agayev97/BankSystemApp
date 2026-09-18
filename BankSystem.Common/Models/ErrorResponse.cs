using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Common.Models
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Instance { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public IDictionary<string, string[]>? Errors { get; set; }
    }
}
