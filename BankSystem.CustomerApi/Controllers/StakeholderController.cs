using BankSystem.CustomerApi.DTOs.Stakeholder;
using BankSystem.CustomerApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.CustomerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StakeholderController : ControllerBase
    {
        private readonly IStakeholderService _stakeholderService;

        public StakeholderController(IStakeholderService stakeholderService)
        {
            _stakeholderService = stakeholderService;
        }

        // GET: api/Stakeholder/legal-entity/1
        [HttpGet("legal-entity/{legalEntityId:int}")]
        public async Task<IActionResult> GetByLegalEntityId(int legalEntityId)
        {
            var stakeholders = await _stakeholderService.GetByLegalEntityIdAsync(legalEntityId);
            return Ok(stakeholders);
        }

      
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStakeholderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _stakeholderService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetByLegalEntityId),
                new { legalEntityId = dto.LegalEntityId },
                new { Id = id, Message = "Stakeholder uğurla yaradıldı." }
            );
        }

       
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStakeholderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _stakeholderService.UpdateAsync(id, dto);

            if (!result)
            {
                return NotFound(new { Message = $"ID-si {id} olan Stakeholder tapılmadı." });
            }

            return Ok(new { Message = "Stakeholder uğurla yeniləndi." });
        }

       
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _stakeholderService.DeleteAsync(id);

            if (!result)
            {
                return NotFound(new { Message = $"ID-si {id} olan Stakeholder tapılmadı." });
            }

            return Ok(new { Message = "Stakeholder deaktiv edildi." });
        }
    }
}