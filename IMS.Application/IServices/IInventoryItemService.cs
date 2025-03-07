using IMS.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.IServices
{
    public interface IInventoryItemService
    {
        Task<IEnumerable<InventoryItemDto>> GetAllInventoryItemsAsync();
        Task<InventoryItemDto> GetInventoryItemByIdAsync(Guid id);
        Task AddInventoryItemAsync(InventoryItemDto itemDto);
        Task UpdateInventoryItemAsync(Guid id, InventoryItemDto itemDto);
        Task DeleteInventoryItemAsync(Guid id);
    }
}
