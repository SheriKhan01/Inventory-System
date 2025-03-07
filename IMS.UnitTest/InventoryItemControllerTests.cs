using Xunit;
using Moq;
using IMS.API.Controllers;
using IMS.Application.IServices;
using IMS.Application.Dtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMS.Tests
{
    public class InventoryItemControllerTests
    {
        private readonly Mock<IInventoryItemService> _inventoryItemServiceMock;
        private readonly Mock<ILogger<InventoryItemController>> _loggerMock;
        private readonly Mock<IMemoryCache> _memoryCacheMock;
        private readonly InventoryItemController _inventoryItemController;

        public InventoryItemControllerTests()
        {
            _inventoryItemServiceMock = new Mock<IInventoryItemService>();
            _loggerMock = new Mock<ILogger<InventoryItemController>>();
            _memoryCacheMock = new Mock<IMemoryCache>();

            _inventoryItemController = new InventoryItemController(
                _inventoryItemServiceMock.Object,
                _loggerMock.Object,
                _memoryCacheMock.Object
            );
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WhenItemsExist()
        {
            var items = new List<InventoryItemDto>
            {
                new InventoryItemDto { Id = Guid.NewGuid(), Name = "Item1" },
                new InventoryItemDto { Id = Guid.NewGuid(), Name = "Item2" }
            };
            _inventoryItemServiceMock.Setup(service => service.GetAllInventoryItemsAsync()).ReturnsAsync(items);

            var result = await _inventoryItemController.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnItems = Assert.IsType<List<InventoryItemDto>>(okResult.Value);
            Assert.Equal(2, returnItems.Count);
        }

        [Fact]
        public async Task GetAll_ReturnsNotFound_WhenNoItemsExist()
        {
            _inventoryItemServiceMock.Setup(service => service.GetAllInventoryItemsAsync()).ReturnsAsync(new List<InventoryItemDto>());

            var result = await _inventoryItemController.GetAll();

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WhenItemExists()
        {
            var itemId = Guid.NewGuid();
            var item = new InventoryItemDto { Id = itemId, Name = "Item1" };
            _inventoryItemServiceMock.Setup(service => service.GetInventoryItemByIdAsync(itemId)).ReturnsAsync(item);

            var result = await _inventoryItemController.GetById(itemId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnItem = Assert.IsType<InventoryItemDto>(okResult.Value);
            Assert.Equal(itemId, returnItem.Id);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenItemDoesNotExist()
        {
            var itemId = Guid.NewGuid();
            _inventoryItemServiceMock.Setup(service => service.GetInventoryItemByIdAsync(itemId)).ThrowsAsync(new KeyNotFoundException());

            var result = await _inventoryItemController.GetById(itemId);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task AddInventory_ReturnsOkResult_WhenItemAdded()
        {
            var itemDto = new InventoryItemDto { Name = "NewItem" };
            _inventoryItemServiceMock.Setup(service => service.AddInventoryItemAsync(itemDto)).Returns(Task.CompletedTask);

            var result = await _inventoryItemController.Create(itemDto);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateInventory_ReturnsOkResult_WhenItemUpdated()
        {
            var itemId = Guid.NewGuid();
            var itemDto = new InventoryItemDto { Name = "UpdatedItem" };
            _inventoryItemServiceMock.Setup(service => service.UpdateInventoryItemAsync(itemId, itemDto)).Returns(Task.CompletedTask);

            var result = await _inventoryItemController.Update(itemId, itemDto);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteInventory_ReturnsOkResult_WhenItemDeleted()
        {
            var itemId = Guid.NewGuid();
            _inventoryItemServiceMock.Setup(service => service.DeleteInventoryItemAsync(itemId)).Returns(Task.CompletedTask);

            var result = await _inventoryItemController.Delete(itemId);

            Assert.IsType<OkResult>(result);
        }
    }
}
