using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using WMS.Model;

namespace WMS.DAL.Repositories
{
    public class WarehouseRepository : IRepository<Warehouse>
    {
        private readonly IDbConnection _connection;

        public WarehouseRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Warehouse>> GetAllAsync()
        {
            const string sql = @"
                SELECT Id, Code, Name, Location, Manager, Contact, Capacity, 
                       Description, IsActive, CreateTime, UpdateTime
                FROM Warehouse
                WHERE IsActive = 1
                ORDER BY Code";

            return await _connection.QueryAsync<Warehouse>(sql);
        }

        public async Task<Warehouse> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT Id, Code, Name, Location, Manager, Contact, Capacity, 
                       Description, IsActive, CreateTime, UpdateTime
                FROM Warehouse
                WHERE Id = @Id";

            return await _connection.QueryFirstOrDefaultAsync<Warehouse>(sql, new { Id = id });
        }

        public async Task<Warehouse> GetByCodeAsync(string code)
        {
            const string sql = @"
                SELECT Id, Code, Name, Location, Manager, Contact, Capacity, 
                       Description, IsActive, CreateTime, UpdateTime
                FROM Warehouse
                WHERE Code = @Code";

            return await _connection.QueryFirstOrDefaultAsync<Warehouse>(sql, new { Code = code });
        }

        public async Task<IEnumerable<Warehouse>> SearchAsync(string searchText)
        {
            const string sql = @"
                SELECT Id, Code, Name, Location, Manager, Contact, Capacity, 
                       Description, IsActive, CreateTime, UpdateTime
                FROM Warehouse
                WHERE (Code LIKE @SearchText OR Name LIKE @SearchText OR Location LIKE @SearchText)
                  AND IsActive = 1
                ORDER BY Code";

            return await _connection.QueryAsync<Warehouse>(sql, new { SearchText = $"%{searchText}%" });
        }

        public async Task<int> AddAsync(Warehouse entity)
        {
            const string sql = @"
                INSERT INTO Warehouse (Code, Name, Location, Manager, Contact, Capacity, 
                                      Description, IsActive, CreateTime)
                VALUES (@Code, @Name, @Location, @Manager, @Contact, @Capacity, 
                        @Description, @IsActive, @CreateTime);
                SELECT SCOPE_IDENTITY();";

            entity.CreateTime = DateTime.Now;
            
            return await _connection.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task UpdateAsync(Warehouse entity)
        {
            const string sql = @"
                UPDATE Warehouse
                SET Code = @Code,
                    Name = @Name,
                    Location = @Location,
                    Manager = @Manager,
                    Contact = @Contact,
                    Capacity = @Capacity,
                    Description = @Description,
                    IsActive = @IsActive,
                    UpdateTime = @UpdateTime
                WHERE Id = @Id";

            entity.UpdateTime = DateTime.Now;
            
            await _connection.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = @"
                UPDATE Warehouse
                SET IsActive = 0,
                    UpdateTime = @UpdateTime
                WHERE Id = @Id";

            await _connection.ExecuteAsync(sql, new { Id = id, UpdateTime = DateTime.Now });
        }

        public async Task<bool> CanDeleteAsync(int id)
        {
            // 检查是否有库存记录引用了该仓库
            const string sql = @"
                SELECT COUNT(1) 
                FROM Inventory 
                WHERE WarehouseId = @Id";

            int count = await _connection.ExecuteScalarAsync<int>(sql, new { Id = id });
            return count == 0;
        }
    }
} 