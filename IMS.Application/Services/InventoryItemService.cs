using AutoMapper;
using IMS.Application.Dtos;
using IMS.Application.IServices;
using IMS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace IMS.Application.Services
{
    public class InventoryItemService : IInventoryItemService
    {
        private readonly IRepository<InventoryItem> _inventoryItemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<InventoryItemService> _logger;

        public InventoryItemService(IRepository<InventoryItem> inventoryItemRepository, IMapper mapper, ILogger<InventoryItemService> logger)
        {
            _inventoryItemRepository = inventoryItemRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<InventoryItemDto>> GetAllInventoryItemsAsync()
        {
            _logger.LogInformation("Fetching all inventory items...");
            var items = await _inventoryItemRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<InventoryItemDto>>(items);
        }

        public async Task<InventoryItemDto> GetInventoryItemByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching inventory item with ID: {Id}", id);
            var item = await _inventoryItemRepository.GetByIdAsync(id);
            return _mapper.Map<InventoryItemDto>(item);
        }

        public async Task AddInventoryItemAsync(InventoryItemDto itemDto)
        {
            _logger.LogInformation("Adding new inventory item: {Name}", itemDto.Name);
            var item = _mapper.Map<InventoryItem>(itemDto);
            await _inventoryItemRepository.AddAsync(item);
        }

        public async Task UpdateInventoryItemAsync(Guid id, InventoryItemDto itemDto)
        {
            _logger.LogInformation("Updating inventory item with ID: {Id}", id);
            var existingItem = await _inventoryItemRepository.GetByIdAsync(id);
            if (existingItem == null) throw new KeyNotFoundException("Inventory item not found");

            _mapper.Map(itemDto, existingItem);
            _inventoryItemRepository.Update(existingItem);
        }

        public async Task DeleteInventoryItemAsync(Guid id)
        {
            _logger.LogInformation("Deleting inventory item with ID: {Id}", id);
            var item = await _inventoryItemRepository.GetByIdAsync(id);
            if (item == null) throw new KeyNotFoundException("Inventory item not found");

            _inventoryItemRepository.Delete(item);
        }
    }

}
