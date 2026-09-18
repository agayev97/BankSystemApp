using BankSystem.CustomerApi.DTOs.Individual;
using BankSystem.CustomerApi.DTOs.LegalEntity;
using BankSystem.CustomerApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.CustomerApi.Controllers
{
    [Authorize] // Ümumi qoruma: Token mütləq olmalıdır
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // 1. Bütün müştəriləri ancaq Admin və İşçilər görə bilər (Müştəri görə bilməz!)
        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        // 2. Müştəri axtarışı (Admin və İşçilər üçün)
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound(new { Message = $"ID-si {id} olan müştəri tapılmadı." });

            return Ok(customer);
        }

        // 3. Fiziki şəxs yaratmaq (Admin və İşçi)
        [HttpPost("individual")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> CreateIndividual([FromBody] CreateIndividualDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int newCustomerId = await _customerService.CreateIndividualCustomerAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = newCustomerId }, new { CustomerId = newCustomerId, Message = "Fiziki şəxs uğurla yaradıldı." });
        }

        // 4. Hüquqi şəxs yaratmaq (Admin və İşçi)
        [HttpPost("legal-entity")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> CreateLegalEntity([FromBody] CreateLegalEntityDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int newCustomerId = await _customerService.CreateLegalEntityCustomerAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = newCustomerId }, new { CustomerId = newCustomerId, Message = "Hüquqi şəxs uğurla yaradıldı." });
        }

        // 5. Müştərini deaktiv etmək (YALNIZ Admin!)
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeactivateCustomer(int id)
        {
            bool isSuccess = await _customerService.DeactivateCustomerAsync(id);
            if (!isSuccess)
                return NotFound(new { Message = $"ID-si {id} olan müştəri tapılmadı və ya artıq deaktivdir." });

            return Ok(new { Message = "Müştəri uğurla deaktiv edildi." });
        }

        // 6. Müştərinin yalnız ÖZ profilini görməsi (Yalnız Customer rolu üçün)
        [HttpGet("my-profile")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyProfile()
        {
            // Giriş etmiş şəxsin tokenindən onun CustomerId-sini oxuyuruq
            var customerIdClaim = User.FindFirst("CustomerId")?.Value;

            if (string.IsNullOrEmpty(customerIdClaim) || !int.TryParse(customerIdClaim, out int customerId))
            {
                return BadRequest(new { Message = "Bu istifadəçi heç bir müştəriyə bağlı deyil." });
            }

            // Yalnız həmin müştərinin məlumatlarını bazadan gətiririk
            var customer = await _customerService.GetCustomerByIdAsync(customerId);
            if (customer == null)
                return NotFound(new { Message = "Profil məlumatı tapılmadı." });

            return Ok(customer);
        }
    }
}