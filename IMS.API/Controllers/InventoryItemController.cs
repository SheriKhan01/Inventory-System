using IMS.Application.Dtos;
using IMS.Application.IServices;
using IMS.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace IMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemController : ControllerBase
    {
        private readonly IInventoryItemService _inventoryItemService;
        private readonly ILogger<InventoryItemController> _logger;
        private readonly IMemoryCache _cacheService; 

        public InventoryItemController(IInventoryItemService inventoryItemService,
                                        ILogger<InventoryItemController> logger,
                                        IMemoryCache cacheService) 
        {
            _inventoryItemService = inventoryItemService;
            _logger = logger;
            _cacheService = cacheService;
        }

        [HttpGet]
        [Route("GetAllInventories")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all inventory items...");
            var cacheKey = "inventoryItems";
            var cachedItems = _cacheService.Get<IEnumerable<InventoryItemDto>>(cacheKey);

            if (cachedItems != null)
            {
                _logger.LogInformation("Found inventory items in cache.");
                return Ok(cachedItems);
            }

            var items = await _inventoryItemService.GetAllInventoryItemsAsync();

            if (!items.Any())
            {
                _logger.LogWarning("No inventory items found.");
                return NotFound("No inventory items available.");
            }

            _cacheService.Set(cacheKey, items, TimeSpan.FromMinutes(10)); 
            return Ok(items);
        }

        [HttpGet]
        [Route("GetInventoryDetail")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Fetching inventory item with ID: {Id}", id);
            var cacheKey = $"inventoryItem-{id}";
            var cachedItem = _cacheService.Get<InventoryItemDto>(cacheKey);

            if (cachedItem != null)
            {
                _logger.LogInformation("Found inventory item with ID: {Id} in cache.", id);
                return Ok(cachedItem);
            }
            var item = await _inventoryItemService.GetInventoryItemByIdAsync(id);

            if (item == null)
            {
                _logger.LogWarning("Inventory item with ID {Id} not found.", id);
                return NotFound($"Item with ID {id} not found.");
            }

            _cacheService.Set(cacheKey, item, TimeSpan.FromMinutes(10));
            return Ok(item);
        }

        [HttpPost]
        [Route("AddInventory")]
        public async Task<IActionResult> Create([FromBody] InventoryItemDto itemDto)
        {
            if (itemDto == null)
            {
                _logger.LogError("Invalid inventory item data provided.");
                return BadRequest("Invalid item data.");
            }

            _logger.LogInformation("Creating new inventory item: {Name}", itemDto.Name);
            await _inventoryItemService.AddInventoryItemAsync(itemDto);
            _cacheService.Remove("inventoryItems");
            return Ok();
        }

        [HttpPut]
        [Route("UpdateInventory")]
        public async Task<IActionResult> Update(Guid id, [FromBody] InventoryItemDto itemDto)
        {
            if (itemDto == null)
            {
                _logger.LogError("Invalid inventory item data for update.");
                return BadRequest("Invalid item data.");
            }

            _logger.LogInformation("Updating inventory item with ID: {Id}", id);
            try
            {
                await _inventoryItemService.UpdateInventoryItemAsync(id, itemDto);
                _cacheService.Remove($"inventoryItem-{id}");
                _cacheService.Remove("inventoryItems"); 
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Inventory item with ID {Id} not found.", id);
                return NotFound($"Item with ID {id} not found.");
            }

            return Ok();
        }

        [HttpDelete]
        [Route("DeleteInventory")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deleting inventory item with ID: {Id}", id);
            try
            {
                await _inventoryItemService.DeleteInventoryItemAsync(id);
                _cacheService.Remove($"inventoryItem-{id}");
                _cacheService.Remove("inventoryItems");
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Inventory item with ID {Id} not found for deletion.", id);
                return NotFound($"Item with ID {id} not found.");
            }

            return Ok();
        }
    }
}
