using IMS.Application.Dtos;
using IMS.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace IMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;
        private readonly IMemoryCache _memoryCache; 

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger, IMemoryCache memoryCache)
        {
            _categoryService = categoryService;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Route("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            _logger.LogInformation("Fetching all categories...");
            if (!_memoryCache.TryGetValue("allCategories", out var cachedCategories))
            {
                var categories = await _categoryService.GetAllCategoriesAsync();

                if (!categories.Any())
                {
                    _logger.LogWarning("No categories found.");
                    return NotFound("No categories available.");
                }
                _memoryCache.Set("allCategories", categories, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

                cachedCategories = categories;
                _logger.LogInformation("Categories fetched from database and cached.");
            }
            else
            {
                _logger.LogInformation("Categories fetched from cache.");
            }

            return Ok(cachedCategories);
        }

        [HttpGet]
        [Route("GetCategoryById")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            _logger.LogInformation("Fetching category with ID: {Id}", id);
            if (!_memoryCache.TryGetValue($"category_{id}", out var cachedCategory))
            {
                try
                {
                    var category = await _categoryService.GetCategoryByIdAsync(id);
                    _memoryCache.Set($"category_{id}", category, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    });

                    cachedCategory = category;
                    _logger.LogInformation("Category fetched from database and cached.");
                }
                catch (KeyNotFoundException ex)
                {
                    _logger.LogWarning(ex, "Category with ID {Id} not found.", id);
                    return NotFound(ex.Message);
                }
            }
            else
            {
                _logger.LogInformation("Category fetched from cache.");
            }

            return Ok(cachedCategory);
        }

        [HttpPost]
        [Route("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                _logger.LogWarning("Attempt to add a null category.");
                return BadRequest("Invalid category data.");
            }

            await _categoryService.AddCategoryAsync(categoryDto);
            _logger.LogInformation("Category '{CategoryName}' added successfully.", categoryDto.Name);
            _memoryCache.Remove("allCategories");

            return Ok();
        }

        [HttpPut]
        [Route("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                _logger.LogWarning("Attempt to update a category with null data.");
                return BadRequest("Invalid category data.");
            }

            try
            {
                await _categoryService.UpdateCategoryAsync(id, categoryDto);
                _logger.LogInformation("Category '{CategoryName}' updated successfully.", categoryDto.Name);
                _memoryCache.Remove("allCategories");
                _memoryCache.Remove($"category_{id}");

                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Category with ID {Id} not found for update.", id);
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                _logger.LogInformation("Category with ID {Id} deleted successfully.", id);
                _memoryCache.Remove("allCategories");
                _memoryCache.Remove($"category_{id}");

                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Category with ID {Id} not found for deletion.", id);
                return NotFound(ex.Message);
            }
        }
    }
}
