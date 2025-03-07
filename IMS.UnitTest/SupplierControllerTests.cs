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
    public class SupplierControllerTests
    {
        private readonly Mock<ISupplierService> _supplierServiceMock;
        private readonly Mock<ILogger<SupplierController>> _loggerMock;
        private readonly Mock<IMemoryCache> _memoryCacheMock;
        private readonly SupplierController _supplierController;

        public SupplierControllerTests()
        {
            _supplierServiceMock = new Mock<ISupplierService>();
            _loggerMock = new Mock<ILogger<SupplierController>>();
            _memoryCacheMock = new Mock<IMemoryCache>();

            _supplierController = new SupplierController(
                _supplierServiceMock.Object,
                _loggerMock.Object,
                _memoryCacheMock.Object
            );
        }

        [Fact]
        public async Task GetAllSuppliers_ReturnsOkResult_WhenSuppliersExist()
        {
            var suppliers = new List<SupplierDto>
            {
                new SupplierDto { Id = Guid.NewGuid(), Name = "Supplier1" },
                new SupplierDto { Id = Guid.NewGuid(), Name = "Supplier2" }
            };
            _supplierServiceMock.Setup(service => service.GetAllSuppliersAsync()).ReturnsAsync(suppliers);

            var result = await _supplierController.GetAllSuppliers();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnSuppliers = Assert.IsType<List<SupplierDto>>(okResult.Value);
            Assert.Equal(2, returnSuppliers.Count);
        }

        [Fact]
        public async Task GetAllSuppliers_ReturnsNotFound_WhenNoSuppliersExist()
        {
            _supplierServiceMock.Setup(service => service.GetAllSuppliersAsync()).ReturnsAsync(new List<SupplierDto>());

            var result = await _supplierController.GetAllSuppliers();

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetSupplierById_ReturnsOkResult_WhenSupplierExists()
        {
            var supplierId = Guid.NewGuid();
            var supplier = new SupplierDto { Id = supplierId, Name = "Supplier1" };
            _supplierServiceMock.Setup(service => service.GetSupplierByIdAsync(supplierId)).ReturnsAsync(supplier);

            var result = await _supplierController.GetSupplierById(supplierId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnSupplier = Assert.IsType<SupplierDto>(okResult.Value);
            Assert.Equal(supplierId, returnSupplier.Id);
        }

        [Fact]
        public async Task GetSupplierById_ReturnsNotFound_WhenSupplierDoesNotExist()
        {
            var supplierId = Guid.NewGuid();
            _supplierServiceMock.Setup(service => service.GetSupplierByIdAsync(supplierId)).ThrowsAsync(new KeyNotFoundException());

            var result = await _supplierController.GetSupplierById(supplierId);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task AddSupplier_ReturnsOkResult_WhenSupplierAdded()
        {
            var supplierDto = new SupplierDto { Name = "NewSupplier" };
            _supplierServiceMock.Setup(service => service.AddSupplierAsync(supplierDto)).Returns(Task.CompletedTask);

            var result = await _supplierController.AddSupplier(supplierDto);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateSupplier_ReturnsOkResult_WhenSupplierUpdated()
        {
            var supplierId = Guid.NewGuid();
            var supplierDto = new SupplierDto { Name = "UpdatedSupplier" };
            _supplierServiceMock.Setup(service => service.UpdateSupplierAsync(supplierId, supplierDto)).Returns(Task.CompletedTask);

            var result = await _supplierController.UpdateSupplier(supplierId, supplierDto);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteSupplier_ReturnsOkResult_WhenSupplierDeleted()
        {
            var supplierId = Guid.NewGuid();
            _supplierServiceMock.Setup(service => service.DeleteSupplierAsync(supplierId)).Returns(Task.CompletedTask);

            var result = await _supplierController.DeleteSupplier(supplierId);

            Assert.IsType<OkResult>(result);
        }
    }
}
