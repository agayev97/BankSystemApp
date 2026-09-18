using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.CustomerApi.DTOs.Individual
{
    public class IndividualResponseDto
    {
        public int CustomerId { get; set; } 
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string? FatherName { get; set; }
        public string? IdentityNo { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
