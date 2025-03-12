using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.DAL.Repositories;
using WMS.Model;

namespace WMS.BLL.Services
{
    public class InventoryService : IService<Inventory>
    {
        private readonly IRepository<Inventory> _inventoryRepository;

        public InventoryService(IRepository<Inventory> inventoryRepository)
        {
            _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
        }

        public async Task<IEnumerable<Inventory>> GetAllAsync()
        {
            return await _inventoryRepository.GetAllAsync();
        }

        public async Task<Inventory> GetByIdAsync(int id)
        {
            return await _inventoryRepository.GetByIdAsync(id);
        }

        public async Task<int> AddAsync(Inventory entity)
        {
            // Add business logic validation here
            if (entity.ProductId <= 0)
            {
                throw new ArgumentException("Invalid product ID");
            }

            if (entity.WarehouseId <= 0)
            {
                throw new ArgumentException("Invalid warehouse ID");
            }

            if (entity.Quantity < 0)
            {
                throw new ArgumentException("Inventory quantity cannot be negative");
            }

            return await _inventoryRepository.AddAsync(entity);
        }

        public async Task<bool> UpdateAsync(Inventory entity)
        {
            // Add business logic validation here
            if (entity.ProductId <= 0)
            {
                throw new ArgumentException("Invalid product ID");
            }

            if (entity.WarehouseId <= 0)
            {
                throw new ArgumentException("Invalid warehouse ID");
            }

            if (entity.Quantity < 0)
            {
                throw new ArgumentException("Inventory quantity cannot be negative");
            }

            return await _inventoryRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // Add business logic validation here
            // For example, check if the inventory is used in any orders
            
            return await _inventoryRepository.DeleteAsync(id);
        }
    }
} 