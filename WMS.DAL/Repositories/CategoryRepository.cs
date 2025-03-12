using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using WMS.Model;

namespace WMS.DAL.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly DatabaseConnection _dbConnection;

        public CategoryRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                const string sql = @"
                    SELECT c.*, p.Name as ParentName
                    FROM Categories c
                    LEFT JOIN Categories p ON c.ParentId = p.Id
                    ORDER BY c.Name";
                
                var categories = await connection.QueryAsync<Category>(sql);
                return categories;
            }
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                const string sql = @"
                    SELECT c.*, p.Name as ParentName
                    FROM Categories c
                    LEFT JOIN Categories p ON c.ParentId = p.Id
                    WHERE c.Id = @Id";
                
                var category = await connection.QueryFirstOrDefaultAsync<Category>(sql, new { Id = id });
                return category;
            }
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                const string sql = @"
                    SELECT c.*, p.Name as ParentName
                    FROM Categories c
                    LEFT JOIN Categories p ON c.ParentId = p.Id
                    WHERE c.Name = @Name";
                
                var category = await connection.QueryFirstOrDefaultAsync<Category>(sql, new { Name = name });
                return category;
            }
        }

        public async Task<int> AddAsync(Category entity)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                const string sql = @"
                    INSERT INTO Categories (
                        Name, Description, ParentId, IsActive, CreatedTime, UpdatedTime
                    ) VALUES (
                        @Name, @Description, @ParentId, @IsActive, @CreatedTime, @UpdatedTime
                    );
                    SELECT CAST(SCOPE_IDENTITY() as int)";
                
                var id = await connection.ExecuteScalarAsync<int>(sql, entity);
                return id;
            }
        }

        public async Task<bool> UpdateAsync(Category entity)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                const string sql = @"
                    UPDATE Categories SET
                        Name = @Name,
                        Description = @Description,
                        ParentId = @ParentId,
                        IsActive = @IsActive,
                        UpdatedTime = @UpdatedTime
                    WHERE Id = @Id";
                
                var affectedRows = await connection.ExecuteAsync(sql, entity);
                return affectedRows > 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                const string sql = "DELETE FROM Categories WHERE Id = @Id";
                var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
                return affectedRows > 0;
            }
        }

        public async Task<bool> HasChildrenAsync(int id)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                const string sql = "SELECT COUNT(1) FROM Categories WHERE ParentId = @Id";
                var count = await connection.ExecuteScalarAsync<int>(sql, new { Id = id });
                return count > 0;
            }
        }

        public async Task<bool> HasProductsAsync(int id)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                const string sql = "SELECT COUNT(1) FROM Products WHERE CategoryId = @Id";
                var count = await connection.ExecuteScalarAsync<int>(sql, new { Id = id });
                return count > 0;
            }
        }
    }
} 