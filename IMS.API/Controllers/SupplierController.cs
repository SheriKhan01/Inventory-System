using IMS.Application.Dtos;
using IMS.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace IMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly ILogger<SupplierController> _logger;
        private readonly IMemoryCache _cacheService;

        public SupplierController(ISupplierService supplierService, ILogger<SupplierController> logger, IMemoryCache cacheService)
        {
            _supplierService = supplierService;
            _logger = logger;
            _cacheService = cacheService;
        }

        [HttpGet]
        [Route("GetAllSupplier")]
        public async Task<IActionResult> GetAllSuppliers()
        {
            _logger.LogInformation("Fetching all suppliers...");
            var cacheKey = "suppliers";
            var cachedSuppliers = _cacheService.Get<IEnumerable<SupplierDto>>(cacheKey);

            if (cachedSuppliers != null)
            {
                _logger.LogInformation("Found suppliers in cache.");
                return Ok(cachedSuppliers);
            }

            var suppliers = await _supplierService.GetAllSuppliersAsync();

            if (!suppliers.Any())
            {
                _logger.LogWarning("No suppliers found.");
                return NotFound("No suppliers available.");
            }

            _cacheService.Set(cacheKey, suppliers, TimeSpan.FromMinutes(10)); 
            return Ok(suppliers);
        }

        [HttpGet]
        [Route("GetSupplieById")]
        public async Task<IActionResult> GetSupplierById(Guid id)
        {
            _logger.LogInformation("Fetching supplier with ID: {Id}", id);
            var cacheKey = $"supplier-{id}";
            var cachedSupplier = _cacheService.Get<SupplierDto>(cacheKey);

            if (cachedSupplier != null)
            {
                _logger.LogInformation("Found supplier with ID: {Id} in cache.", id);
                return Ok(cachedSupplier);
            }

            try
            {
                var supplier = await _supplierService.GetSupplierByIdAsync(id);

                if (supplier == null)
                {
                    _logger.LogWarning("Supplier with ID {Id} not found.", id);
                    return NotFound($"Supplier with ID {id} not found.");
                }

                _cacheService.Set(cacheKey, supplier, TimeSpan.FromMinutes(10)); 
                return Ok(supplier);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Supplier with ID {Id} not found.", id);
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddSupplier")]
        public async Task<IActionResult> AddSupplier([FromBody] SupplierDto supplierDto)
        {
            if (supplierDto == null)
            {
                _logger.LogWarning("Attempt to add a null supplier.");
                return BadRequest("Invalid supplier data.");
            }

            await _supplierService.AddSupplierAsync(supplierDto);
            _cacheService.Remove("suppliers"); 
            _logger.LogInformation("Supplier '{SupplierName}' added successfully.", supplierDto.Name);

            return Ok();
        }

        [HttpPut]
        [Route("UpdateSupplier")]
        public async Task<IActionResult> UpdateSupplier(Guid id, [FromBody] SupplierDto supplierDto)
        {
            if (supplierDto == null)
            {
                _logger.LogWarning("Attempt to update a supplier with null data.");
                return BadRequest("Invalid supplier data.");
            }

            try
            {
                await _supplierService.UpdateSupplierAsync(id, supplierDto);
                _cacheService.Remove($"supplier-{id}"); 
                _cacheService.Remove("suppliers"); 
                _logger.LogInformation("Supplier '{SupplierName}' updated successfully.", supplierDto.Name);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Supplier with ID {Id} not found for update.", id);
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteSupplier")]
        public async Task<IActionResult> DeleteSupplier(Guid id)
        {
            _logger.LogInformation("Deleting supplier with ID: {Id}", id);
            try
            {
                await _supplierService.DeleteSupplierAsync(id);
                _cacheService.Remove($"supplier-{id}");
                _cacheService.Remove("suppliers"); 
                _logger.LogInformation("Supplier with ID {Id} deleted successfully.", id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Supplier with ID {Id} not found for deletion.", id);
                return NotFound(ex.Message);
            }
        }
    }
}
