using System;

namespace WMS.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // 存储加密后的密码
        public string Salt { get; set; } // 密码盐值
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
        public int LoginFailedCount { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
    
    public enum UserRole
    {
        Administrator = 1,
        Manager = 2,
        Operator = 3,
        Viewer = 4
    }
} 