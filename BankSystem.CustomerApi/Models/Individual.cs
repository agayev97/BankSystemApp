using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.CustomerApi.Models
{
    public class Individual
    {
        public int CustomerId { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? FatherName { get; set; }
        public string IdentityNo { get; set; }  = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
