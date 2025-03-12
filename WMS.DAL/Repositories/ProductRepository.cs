using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WMS.Model;

namespace WMS.DAL.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly DatabaseConnection _dbConnection;

        public ProductRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                const string sql = @"
                    SELECT p.*, c.Name as CategoryName
                    FROM Products p
                    LEFT JOIN Categories c ON p.CategoryId = c.Id
                    ORDER BY p.Code";
                
                var products = await connection.QueryAsync<Product>(sql);
                return products;
            }
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                const string sql = @"
                    SELECT p.*, c.Name as CategoryName
                    FROM Products p
                    LEFT JOIN Categories c ON p.CategoryId = c.Id
                    WHERE p.Id = @Id";
                
                var product = await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
                return product;
            }
        }

        public async Task<Product> GetByCodeAsync(string code)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                const string sql = @"
                    SELECT p.*, c.Name as CategoryName
                    FROM Products p
                    LEFT JOIN Categories c ON p.CategoryId = c.Id
                    WHERE p.Code = @Code";
                
                var product = await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Code = code });
                return product;
            }
        }

        public async Task<IEnumerable<Product>> SearchAsync(string searchText)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                const string sql = @"
                    SELECT p.*, c.Name as CategoryName
                    FROM Products p
                    LEFT JOIN Categories c ON p.CategoryId = c.Id
                    WHERE p.Code LIKE @SearchText OR p.Name LIKE @SearchText OR p.Specification LIKE @SearchText
                    ORDER BY p.Code";
                
                var products = await connection.QueryAsync<Product>(sql, new { SearchText = $"%{searchText}%" });
                return products;
            }
        }

        public async Task<int> AddAsync(Product entity)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                const string sql = @"
                    INSERT INTO Products (
                        Code, Name, Specification, CategoryId, UnitPrice, Unit, 
                        MinStock, MaxStock, Description, IsActive, CreatedTime, UpdatedTime
                    ) VALUES (
                        @Code, @Name, @Specification, @CategoryId, @UnitPrice, @Unit, 
                        @MinStock, @MaxStock, @Description, @IsActive, @CreatedTime, @UpdatedTime
                    );
                    SELECT CAST(SCOPE_IDENTITY() as int)";
                
                var id = await connection.ExecuteScalarAsync<int>(sql, entity);
                return id;
            }
        }

        public async Task<bool> UpdateAsync(Product entity)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                const string sql = @"
                    UPDATE Products SET
                        Code = @Code,
                        Name = @Name,
                        Specification = @Specification,
                        CategoryId = @CategoryId,
                        UnitPrice = @UnitPrice,
                        Unit = @Unit,
                        MinStock = @MinStock,
                        MaxStock = @MaxStock,
                        Description = @Description,
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
                const string sql = "DELETE FROM Products WHERE Id = @Id";
                var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
                return affectedRows > 0;
            }
        }

        public async Task<bool> CanDeleteAsync(int id)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                // 检查是否有库存记录
                const string sql = @"
                    SELECT COUNT(1) FROM Inventory WHERE ProductId = @Id
                    UNION ALL
                    SELECT COUNT(1) FROM InboundDetails WHERE ProductId = @Id
                    UNION ALL
                    SELECT COUNT(1) FROM OutboundDetails WHERE ProductId = @Id";
                
                var counts = await connection.QueryAsync<int>(sql, new { Id = id });
                return counts.All(count => count == 0);
            }
        }
    }
} 