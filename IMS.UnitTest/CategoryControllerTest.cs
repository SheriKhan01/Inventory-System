using IMS.API.Controllers;
using IMS.Application.Dtos;
using IMS.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UnitTest
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly Mock<ILogger<CategoryController>> _loggerMock;
        private readonly Mock<IMemoryCache> _memoryCacheMock;
        private readonly CategoryController _categoryController;

        public CategoryControllerTests()
        {
            _categoryServiceMock = new Mock<ICategoryService>();
            _loggerMock = new Mock<ILogger<CategoryController>>();
            _memoryCacheMock = new Mock<IMemoryCache>(); 
            _categoryController = new CategoryController(
                _categoryServiceMock.Object,
                _loggerMock.Object,
                _memoryCacheMock.Object
            );
        }

        [Fact]
        public async Task GetAllCategories_ReturnsOkResult_WhenCategoriesExist()
        {
            var categories = new List<CategoryDto>
            {
                new CategoryDto { Id = Guid.NewGuid(), Name = "Category1" },
                new CategoryDto { Id = Guid.NewGuid(), Name = "Category2" }
            };
            _categoryServiceMock.Setup(service => service.GetAllCategoriesAsync()).ReturnsAsync(categories);
            var result = await _categoryController.GetAllCategories();
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnCategories = Assert.IsType<List<CategoryDto>>(okResult.Value);
            Assert.Equal(2, returnCategories.Count);
        }

        [Fact]
        public async Task GetAllCategories_ReturnsNotFound_WhenNoCategoriesExist()
        {
            _categoryServiceMock.Setup(service => service.GetAllCategoriesAsync()).ReturnsAsync(new List<CategoryDto>());
            var result = await _categoryController.GetAllCategories();
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetCategoryById_ReturnsOkResult_WhenCategoryExists()
        {
            var categoryId = Guid.NewGuid();
            var category = new CategoryDto { Id = categoryId, Name = "Category1" };
            _categoryServiceMock.Setup(service => service.GetCategoryByIdAsync(categoryId)).ReturnsAsync(category);
            var result = await _categoryController.GetCategoryById(categoryId);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnCategory = Assert.IsType<CategoryDto>(okResult.Value);
            Assert.Equal(categoryId, returnCategory.Id);
        }

        [Fact]
        public async Task GetCategoryById_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            var categoryId = Guid.NewGuid();
            _categoryServiceMock.Setup(service => service.GetCategoryByIdAsync(categoryId)).ThrowsAsync(new KeyNotFoundException());
            var result = await _categoryController.GetCategoryById(categoryId);
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
