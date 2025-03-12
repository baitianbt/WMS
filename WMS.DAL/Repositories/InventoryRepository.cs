using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WMS.Model;

namespace WMS.DAL.Repositories
{
    public class InventoryRepository : IRepository<Inventory>
    {
        private readonly DatabaseConnection _dbConnection;

        public InventoryRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<IEnumerable<Inventory>> GetAllAsync()
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                var sql = @"
                    SELECT i.*, p.*, w.*
                    FROM Inventories i
                    INNER JOIN Products p ON i.ProductId = p.Id
                    INNER JOIN Warehouses w ON i.WarehouseId = w.Id";

                var inventoryDictionary = new Dictionary<int, Inventory>();

                await connection.QueryAsync<Inventory, Product, Warehouse, Inventory>(
                    sql,
                    (inventory, product, warehouse) =>
                    {
                        if (!inventoryDictionary.TryGetValue(inventory.Id, out var existingInventory))
                        {
                            existingInventory = inventory;
                            existingInventory.Product = product;
                            existingInventory.Warehouse = warehouse;
                            inventoryDictionary.Add(existingInventory.Id, existingInventory);
                        }

                        return existingInventory;
                    },
                    splitOn: "Id,Id");

                return inventoryDictionary.Values;
            }
        }

        public async Task<Inventory> GetByIdAsync(int id)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                var sql = @"
                    SELECT i.*, p.*, w.*
                    FROM Inventories i
                    INNER JOIN Products p ON i.ProductId = p.Id
                    INNER JOIN Warehouses w ON i.WarehouseId = w.Id
                    WHERE i.Id = @Id";

                var inventories = await connection.QueryAsync<Inventory, Product, Warehouse, Inventory>(
                    sql,
                    (inventory, product, warehouse) =>
                    {
                        inventory.Product = product;
                        inventory.Warehouse = warehouse;
                        return inventory;
                    },
                    new { Id = id },
                    splitOn: "Id,Id");

                return inventories.FirstOrDefault();
            }
        }

        public async Task<int> AddAsync(Inventory entity)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                var sql = @"
                    INSERT INTO Inventories (ProductId, WarehouseId, Location, Quantity, MinQuantity, MaxQuantity, CreatedTime, UpdatedTime)
                    VALUES (@ProductId, @WarehouseId, @Location, @Quantity, @MinQuantity, @MaxQuantity, @CreatedTime, @UpdatedTime);
                    SELECT CAST(SCOPE_IDENTITY() as int)";

                entity.CreatedTime = DateTime.Now;
                entity.UpdatedTime = DateTime.Now;

                return await connection.QuerySingleAsync<int>(sql, entity);
            }
        }

        public async Task<bool> UpdateAsync(Inventory entity)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                var sql = @"
                    UPDATE Inventories 
                    SET ProductId = @ProductId, 
                        WarehouseId = @WarehouseId, 
                        Location = @Location, 
                        Quantity = @Quantity, 
                        MinQuantity = @MinQuantity, 
                        MaxQuantity = @MaxQuantity, 
                        UpdatedTime = @UpdatedTime
                    WHERE Id = @Id";

                entity.UpdatedTime = DateTime.Now;

                var affectedRows = await connection.ExecuteAsync(sql, entity);
                return affectedRows > 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                var sql = "DELETE FROM Inventories WHERE Id = @Id";
                var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
                return affectedRows > 0;
            }
        }
    }
} 