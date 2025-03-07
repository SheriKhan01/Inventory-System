using IMS.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.IServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> GetCategoryByIdAsync(Guid id);
        Task AddCategoryAsync(CategoryDto categoryDto);
        Task UpdateCategoryAsync(Guid id, CategoryDto categoryDto);
        Task DeleteCategoryAsync(Guid id);
    }

}
