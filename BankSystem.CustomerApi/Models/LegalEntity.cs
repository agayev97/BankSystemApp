using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.CustomerApi.Models
{
    public class LegalEntity
    {
        public int CustomerId { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public string Voen { get; set; } = string.Empty;
        public string? EntityType { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
