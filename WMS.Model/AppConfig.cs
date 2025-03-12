using System;
using System.Collections.Generic;

namespace WMS.Model
{
    public class AppConfig
    {
        // 数据库配置
        public string ConnectionString { get; set; }
        
        // 日志配置
        public LogConfig LogConfig { get; set; }
        
        // 系统配置
        public string SystemName { get; set; }
        public string Version { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        
        // 用户配置
        public int PasswordMinLength { get; set; }
        public bool RequireSpecialCharacter { get; set; }
        public bool RequireUppercase { get; set; }
        public bool RequireNumber { get; set; }
        public int LoginFailedLockCount { get; set; }
        
        // 其他配置
        public Dictionary<string, string> CustomSettings { get; set; }
        
        public AppConfig()
        {
            // 默认配置
            LogConfig = new LogConfig();
            CustomSettings = new Dictionary<string, string>();
            PasswordMinLength = 6;
            RequireSpecialCharacter = false;
            RequireUppercase = false;
            RequireNumber = true;
            LoginFailedLockCount = 5;
            SystemName = "WMS仓库管理系统";
            Version = "1.0.0";
        }
    }
    
    public class LogConfig
    {
        public string LogFilePath { get; set; }
        public LogLevel MinimumLogLevel { get; set; }
        public bool LogToFile { get; set; }
        public bool LogToConsole { get; set; }
        public bool LogToDatabase { get; set; }
        public int MaxLogFileSize { get; set; } // 单位：MB
        public int MaxLogFileCount { get; set; }
        
        public LogConfig()
        {
            // 默认配置
            LogFilePath = "Logs";
            MinimumLogLevel = LogLevel.Information;
            LogToFile = true;
            LogToConsole = false;
            LogToDatabase = false;
            MaxLogFileSize = 10; // 10MB
            MaxLogFileCount = 10;
        }
    }
    
    public enum LogLevel
    {
        Trace = 0,
        Debug = 1,
        Information = 2,
        Warning = 3,
        Error = 4,
        Critical = 5,
        None = 6
    }
} 