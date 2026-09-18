using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.CustomerApi.DTOs.Stakeholder
{
    public class UpdateStakeholderDto
    {
        public int LegalEntityId { get; set; }
        public string PartyType { get; set; } = string.Empty;
        public string RelationType { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? IdentityNumber { get; set; }
        public string? Voen { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
    }
}
