using IMS.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.IServices
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync();
        Task<SupplierDto> GetSupplierByIdAsync(Guid id);
        Task AddSupplierAsync(SupplierDto supplierDto);
        Task UpdateSupplierAsync(Guid id, SupplierDto supplierDto);
        Task DeleteSupplierAsync(Guid id);
    }
}
