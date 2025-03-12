using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using WMS.Model;

namespace WMS.DAL.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly DatabaseConnection _dbConnection;

        public UserRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                return await connection.QueryAsync<User>("SELECT * FROM Users");
            }
        }

        public async Task<User> GetByIdAsync(int id)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM Users WHERE Id = @Id", new { Id = id });
            }
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM Users WHERE Username = @Username", new { Username = username });
            }
        }

        public async Task<int> AddAsync(User entity)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                var sql = @"
                    INSERT INTO Users (Username, Password, Salt, FullName, Email, Phone, Role, IsActive, LoginFailedCount, LastLoginTime, CreatedTime, UpdatedTime)
                    VALUES (@Username, @Password, @Salt, @FullName, @Email, @Phone, @Role, @IsActive, @LoginFailedCount, @LastLoginTime, @CreatedTime, @UpdatedTime);
                    SELECT CAST(SCOPE_IDENTITY() as int)";

                entity.CreatedTime = DateTime.Now;
                entity.UpdatedTime = DateTime.Now;

                return await connection.QuerySingleAsync<int>(sql, entity);
            }
        }

        public async Task<bool> UpdateAsync(User entity)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                var sql = @"
                    UPDATE Users 
                    SET Username = @Username, 
                        Password = @Password, 
                        Salt = @Salt, 
                        FullName = @FullName, 
                        Email = @Email, 
                        Phone = @Phone, 
                        Role = @Role, 
                        IsActive = @IsActive, 
                        LoginFailedCount = @LoginFailedCount, 
                        LastLoginTime = @LastLoginTime, 
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
                var sql = "DELETE FROM Users WHERE Id = @Id";
                var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
                return affectedRows > 0;
            }
        }

        public async Task<bool> UpdateLoginFailedCountAsync(int userId, int loginFailedCount)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                var sql = @"
                    UPDATE Users 
                    SET LoginFailedCount = @LoginFailedCount, 
                        UpdatedTime = @UpdatedTime
                    WHERE Id = @Id";

                var affectedRows = await connection.ExecuteAsync(sql, new 
                { 
                    Id = userId, 
                    LoginFailedCount = loginFailedCount, 
                    UpdatedTime = DateTime.Now 
                });
                
                return affectedRows > 0;
            }
        }

        public async Task<bool> UpdateLastLoginTimeAsync(int userId, DateTime lastLoginTime)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                var sql = @"
                    UPDATE Users 
                    SET LastLoginTime = @LastLoginTime, 
                        LoginFailedCount = 0,
                        UpdatedTime = @UpdatedTime
                    WHERE Id = @Id";

                var affectedRows = await connection.ExecuteAsync(sql, new 
                { 
                    Id = userId, 
                    LastLoginTime = lastLoginTime, 
                    UpdatedTime = DateTime.Now 
                });
                
                return affectedRows > 0;
            }
        }

        public async Task<bool> ChangePasswordAsync(int userId, string newPassword, string salt)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                var sql = @"
                    UPDATE Users 
                    SET Password = @Password, 
                        Salt = @Salt,
                        UpdatedTime = @UpdatedTime
                    WHERE Id = @Id";

                var affectedRows = await connection.ExecuteAsync(sql, new 
                { 
                    Id = userId, 
                    Password = newPassword, 
                    Salt = salt,
                    UpdatedTime = DateTime.Now 
                });
                
                return affectedRows > 0;
            }
        }

        public async Task AddLoginHistoryAsync(int userId, string username, string ipAddress, bool loginStatus, string failReason = null)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                var sql = @"
                    INSERT INTO UserLoginHistory (UserId, Username, LoginTime, IPAddress, LoginStatus, FailReason)
                    VALUES (@UserId, @Username, @LoginTime, @IPAddress, @LoginStatus, @FailReason)";

                await connection.ExecuteAsync(sql, new 
                { 
                    UserId = userId, 
                    Username = username, 
                    LoginTime = DateTime.Now, 
                    IPAddress = ipAddress, 
                    LoginStatus = loginStatus, 
                    FailReason = failReason 
                });
            }
        }

        public async Task UpdateLogoutTimeAsync(int userId, DateTime logoutTime)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                var sql = @"
                    UPDATE UserLoginHistory 
                    SET LogoutTime = @LogoutTime
                    WHERE UserId = @UserId AND LogoutTime IS NULL";

                await connection.ExecuteAsync(sql, new 
                { 
                    UserId = userId, 
                    LogoutTime = logoutTime
                });
            }
        }
    }
} 