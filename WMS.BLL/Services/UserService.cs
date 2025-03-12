using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WMS.DAL.Repositories;
using WMS.Model;

namespace WMS.BLL.Services
{
    public class UserService : IService<User>
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                Logger.Info("获取所有用户");
                return await _userRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                Logger.Error("获取所有用户失败", ex);
                throw;
            }
        }

        public async Task<User> GetByIdAsync(int id)
        {
            try
            {
                Logger.Info($"获取用户，ID: {id}");
                return await _userRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                Logger.Error($"获取用户失败，ID: {id}", ex);
                throw;
            }
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            try
            {
                Logger.Info($"获取用户，用户名: {username}");
                return await _userRepository.GetByUsernameAsync(username);
            }
            catch (Exception ex)
            {
                Logger.Error($"获取用户失败，用户名: {username}", ex);
                throw;
            }
        }

        public async Task<int> AddAsync(User entity)
        {
            try
            {
                // 验证用户名是否已存在
                var existingUser = await _userRepository.GetByUsernameAsync(entity.Username);
                if (existingUser != null)
                {
                    throw new Exception("用户名已存在");
                }

                // 验证密码
                ValidatePassword(entity.Password);

                // 生成盐值和加密密码
                entity.Salt = GenerateSalt();
                entity.Password = HashPassword(entity.Password, entity.Salt);
                entity.LoginFailedCount = 0;
                entity.IsActive = true;

                Logger.Info($"添加用户，用户名: {entity.Username}");
                return await _userRepository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                Logger.Error($"添加用户失败，用户名: {entity.Username}", ex);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(User entity)
        {
            try
            {
                // 获取原始用户信息
                var originalUser = await _userRepository.GetByIdAsync(entity.Id);
                if (originalUser == null)
                {
                    throw new Exception("用户不存在");
                }

                // 如果用户名已更改，验证新用户名是否已存在
                if (entity.Username != originalUser.Username)
                {
                    var existingUser = await _userRepository.GetByUsernameAsync(entity.Username);
                    if (existingUser != null)
                    {
                        throw new Exception("用户名已存在");
                    }
                }

                // 保持原始密码和盐值不变
                entity.Password = originalUser.Password;
                entity.Salt = originalUser.Salt;

                Logger.Info($"更新用户，ID: {entity.Id}，用户名: {entity.Username}");
                return await _userRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                Logger.Error($"更新用户失败，ID: {entity.Id}，用户名: {entity.Username}", ex);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                Logger.Info($"删除用户，ID: {id}");
                return await _userRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                Logger.Error($"删除用户失败，ID: {id}", ex);
                throw;
            }
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            try
            {
                // 获取用户信息
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("用户不存在");
                }

                // 验证旧密码
                string hashedOldPassword = HashPassword(oldPassword, user.Salt);
                if (hashedOldPassword != user.Password)
                {
                    throw new Exception("旧密码不正确");
                }

                // 验证新密码
                ValidatePassword(newPassword);

                // 生成新的盐值和加密密码
                string salt = GenerateSalt();
                string hashedNewPassword = HashPassword(newPassword, salt);

                Logger.Info($"修改密码，用户ID: {userId}");
                return await _userRepository.ChangePasswordAsync(userId, hashedNewPassword, salt);
            }
            catch (Exception ex)
            {
                Logger.Error($"修改密码失败，用户ID: {userId}", ex);
                throw;
            }
        }

        public async Task<bool> ResetPasswordAsync(int userId, string newPassword)
        {
            try
            {
                // 获取用户信息
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("用户不存在");
                }

                // 验证新密码
                ValidatePassword(newPassword);

                // 生成新的盐值和加密密码
                string salt = GenerateSalt();
                string hashedNewPassword = HashPassword(newPassword, salt);

                Logger.Info($"重置密码，用户ID: {userId}");
                return await _userRepository.ChangePasswordAsync(userId, hashedNewPassword, salt);
            }
            catch (Exception ex)
            {
                Logger.Error($"重置密码失败，用户ID: {userId}", ex);
                throw;
            }
        }

        public async Task<User> LoginAsync(string username, string password, string ipAddress)
        {
            try
            {
                // 获取用户信息
                var user = await _userRepository.GetByUsernameAsync(username);
                if (user == null)
                {
                    Logger.Warn($"登录失败，用户名不存在: {username}");
                    await _userRepository.AddLoginHistoryAsync(0, username, ipAddress, false, "用户名不存在");
                    throw new Exception("用户名或密码不正确");
                }

                // 检查用户是否被禁用
                if (!user.IsActive)
                {
                    Logger.Warn($"登录失败，用户已被禁用: {username}");
                    await _userRepository.AddLoginHistoryAsync(user.Id, username, ipAddress, false, "用户已被禁用");
                    throw new Exception("用户已被禁用");
                }

                // 检查登录失败次数
                if (user.LoginFailedCount >= ConfigManager.Config.LoginFailedLockCount)
                {
                    Logger.Warn($"登录失败，用户已被锁定: {username}");
                    await _userRepository.AddLoginHistoryAsync(user.Id, username, ipAddress, false, "用户已被锁定");
                    throw new Exception("用户已被锁定，请联系管理员");
                }

                // 验证密码
                string hashedPassword = HashPassword(password, user.Salt);
                if (hashedPassword != user.Password)
                {
                    // 增加登录失败次数
                    user.LoginFailedCount++;
                    await _userRepository.UpdateLoginFailedCountAsync(user.Id, user.LoginFailedCount);

                    Logger.Warn($"登录失败，密码错误: {username}，失败次数: {user.LoginFailedCount}");
                    await _userRepository.AddLoginHistoryAsync(user.Id, username, ipAddress, false, "密码错误");
                    
                    if (user.LoginFailedCount >= ConfigManager.Config.LoginFailedLockCount)
                    {
                        throw new Exception("用户已被锁定，请联系管理员");
                    }
                    else
                    {
                        throw new Exception("用户名或密码不正确");
                    }
                }

                // 登录成功，更新最后登录时间和重置登录失败次数
                user.LastLoginTime = DateTime.Now;
                await _userRepository.UpdateLastLoginTimeAsync(user.Id, user.LastLoginTime.Value);

                // 记录登录历史
                await _userRepository.AddLoginHistoryAsync(user.Id, username, ipAddress, true);

                Logger.Info($"登录成功，用户名: {username}");
                return user;
            }
            catch (Exception ex)
            {
                // 这里不需要再次记录日志，因为在上面的代码中已经记录了
                throw;
            }
        }

        public async Task LogoutAsync(int userId, DateTime logoutTime)
        {
            try
            {
                Logger.Info($"用户登出，ID: {userId}");
                await _userRepository.UpdateLogoutTimeAsync(userId, logoutTime);
            }
            catch (Exception ex)
            {
                Logger.Error($"记录用户登出失败，ID: {userId}", ex);
                throw;
            }
        }

        #region 辅助方法

        private void ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new Exception("密码不能为空");
            }

            if (password.Length < ConfigManager.Config.PasswordMinLength)
            {
                throw new Exception($"密码长度不能小于{ConfigManager.Config.PasswordMinLength}个字符");
            }

            if (ConfigManager.Config.RequireSpecialCharacter && !password.Any(c => !char.IsLetterOrDigit(c)))
            {
                throw new Exception("密码必须包含特殊字符");
            }

            if (ConfigManager.Config.RequireUppercase && !password.Any(char.IsUpper))
            {
                throw new Exception("密码必须包含大写字母");
            }

            if (ConfigManager.Config.RequireNumber && !password.Any(char.IsDigit))
            {
                throw new Exception("密码必须包含数字");
            }
        }

        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        #endregion
    }
} 