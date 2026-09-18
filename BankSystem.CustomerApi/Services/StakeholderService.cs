using BankSystem.CustomerApi.DTOs.Stakeholder;
using BankSystem.CustomerApi.Models;
using BankSystem.CustomerApi.Repositories.Interfaces;
using BankSystem.CustomerApi.Services.Interfaces;

namespace BankSystem.CustomerApi.Services
{
    public class StakeholderService : IStakeholderService
    {
        private readonly IStakeholderRepository _stakeholderRepository;

        public StakeholderService(IStakeholderRepository stakeholderRepository)
        {
            _stakeholderRepository = stakeholderRepository;
        }

        public async Task<IEnumerable<StakeholderResponseDto>> GetByLegalEntityIdAsync(int legalEntityId)
        {
            var stakeholders = await _stakeholderRepository.GetByLegalEntityIdAsync(legalEntityId);

            return stakeholders.Select(x => new StakeholderResponseDto
            {
                Id = x.Id, // Id DTO-ya əlavə edildi
                LegalEntityId = x.LegalEntityId,
                PartyType = x.PartyType,
                RelationType = x.RelationType,
                FullName = x.FullName,
                IdentityNumber = x.IdentityNumber,
                Voen = x.Voen,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                IsActive = x.IsActive
            });
        }

        public async Task<int> CreateAsync(CreateStakeholderDto dto)
        {
            var stakeholder = new Stakeholder
            {
                LegalEntityId = dto.LegalEntityId,
                PartyType = dto.PartyType,
                RelationType = dto.RelationType,
                FullName = dto.FullName,
                IdentityNumber = dto.IdentityNumber,
                Voen = dto.Voen,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                IsActive = true
            };

            return await _stakeholderRepository.CreateAsync(stakeholder);
        }

        public async Task<bool> UpdateAsync(int id, UpdateStakeholderDto dto)
        {
            var stakeholder = new Stakeholder
            {
                Id = id,
                LegalEntityId = dto.LegalEntityId,
                PartyType = dto.PartyType,
                RelationType = dto.RelationType,
                FullName = dto.FullName,
                IdentityNumber = dto.IdentityNumber,
                Voen = dto.Voen,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                IsActive = dto.IsActive
            };

            return await _stakeholderRepository.UpdateAsync(stakeholder);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _stakeholderRepository.DeleteAsync(id);
        }
    }
}