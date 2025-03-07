using AutoMapper;
using IMS.Application.Dtos;
using IMS.Application.IServices;
using IMS.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Services
{
    public class SupplierService:ISupplierService
    {
        private readonly IRepository<Supplier> _supplierRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SupplierService> _logger;

        public SupplierService(IRepository<Supplier> supplierRepository, IMapper mapper, ILogger<SupplierService> logger)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync()
        {
            _logger.LogInformation("Fetching all suppliers...");
            var suppliers = await _supplierRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
        }

        public async Task<SupplierDto> GetSupplierByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching supplier with ID: {Id}", id);
            var supplier = await _supplierRepository.GetByIdAsync(id);

            if (supplier == null)
            {
                _logger.LogWarning("Supplier with ID {Id} not found.", id);
                throw new KeyNotFoundException($"Supplier with ID {id} not found.");
            }

            return _mapper.Map<SupplierDto>(supplier);
        }

        public async Task AddSupplierAsync(SupplierDto supplierDto)
        {
            var supplier = _mapper.Map<Supplier>(supplierDto);
            await _supplierRepository.AddAsync(supplier);
            _logger.LogInformation("Added new supplier: {SupplierName}", supplierDto.Name);
        }

        public async Task UpdateSupplierAsync(Guid id, SupplierDto supplierDto)
        {
            var existingSupplier = await _supplierRepository.GetByIdAsync(id);

            if (existingSupplier == null)
            {
                _logger.LogWarning("Supplier with ID {Id} not found for update.", id);
                throw new KeyNotFoundException($"Supplier with ID {id} not found.");
            }

            _mapper.Map(supplierDto, existingSupplier);
            _supplierRepository.Update(existingSupplier);
            _logger.LogInformation("Updated supplier: {SupplierName}", supplierDto.Name);
        }

        public async Task DeleteSupplierAsync(Guid id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null)
            {
                _logger.LogWarning("Supplier with ID {Id} not found for deletion.", id);
                throw new KeyNotFoundException($"Supplier with ID {id} not found.");
            }

            _supplierRepository.Delete(supplier);
            _logger.LogInformation("Deleted supplier with ID {Id}", id);
        }
    }
}
