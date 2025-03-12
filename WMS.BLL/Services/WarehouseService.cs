using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.DAL.Repositories;
using WMS.Model;

namespace WMS.BLL.Services
{
    public class WarehouseService : IService<Warehouse>
    {
        private readonly IRepository<Warehouse> _warehouseRepository;

        public WarehouseService(IRepository<Warehouse> warehouseRepository)
        {
            _warehouseRepository = warehouseRepository ?? throw new ArgumentNullException(nameof(warehouseRepository));
        }

        public async Task<IEnumerable<Warehouse>> GetAllAsync()
        {
            return await _warehouseRepository.GetAllAsync();
        }

        public async Task<Warehouse> GetByIdAsync(int id)
        {
            return await _warehouseRepository.GetByIdAsync(id);
        }

        public async Task<int> AddAsync(Warehouse entity)
        {
            // Add business logic validation here
            if (string.IsNullOrEmpty(entity.Code))
            {
                throw new ArgumentException("Warehouse code cannot be empty");
            }

            if (string.IsNullOrEmpty(entity.Name))
            {
                throw new ArgumentException("Warehouse name cannot be empty");
            }

            return await _warehouseRepository.AddAsync(entity);
        }

        public async Task<bool> UpdateAsync(Warehouse entity)
        {
            // Add business logic validation here
            if (string.IsNullOrEmpty(entity.Code))
            {
                throw new ArgumentException("Warehouse code cannot be empty");
            }

            if (string.IsNullOrEmpty(entity.Name))
            {
                throw new ArgumentException("Warehouse name cannot be empty");
            }

            return await _warehouseRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // Add business logic validation here
            // For example, check if the warehouse is used in any inventory or orders
            
            return await _warehouseRepository.DeleteAsync(id);
        }
    }
} 