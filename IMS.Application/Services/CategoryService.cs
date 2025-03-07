using AutoMapper;
using IMS.Application.Dtos;
using IMS.Application.IServices;
using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IRepository<Category> categoryRepository, IMapper mapper, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            _logger.LogInformation("Retrieving all categories with inventory items...");

            var categories = await _categoryRepository.GetAllAsync(include: q => q.Include(c => c.InventoryItems));
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id, include: q => q.Include(c => c.InventoryItems));

            if (category == null)
            {
                _logger.LogWarning("Category with ID {Id} not found.", id);
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task AddCategoryAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryRepository.AddAsync(category);
            _logger.LogInformation("Category '{CategoryName}' added successfully.", category.Name);
        }

        public async Task UpdateCategoryAsync(Guid id, CategoryDto categoryDto)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);

            if (existingCategory == null)
            {
                _logger.LogWarning("Attempt to update non-existing Category with ID {Id}", id);
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }

            _mapper.Map(categoryDto, existingCategory);
            _categoryRepository.Update(existingCategory);
            _logger.LogInformation("Category '{CategoryName}' updated successfully.", existingCategory.Name);
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                _logger.LogWarning("Attempt to delete non-existing Category with ID {Id}", id);
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }

            _categoryRepository.Delete(category);
            _logger.LogInformation("Category '{CategoryName}' deleted successfully.", category.Name);
        }
    }


}
