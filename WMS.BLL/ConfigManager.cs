using System;
using System.IO;
using System.Text.Json;
using WMS.Model;

namespace WMS.BLL
{
    public class ConfigManager
    {
        private static readonly string ConfigFileName = "appsettings.json";
        private static readonly string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFileName);
        private static AppConfig _config;
        
        public static AppConfig Config
        {
            get
            {
                if (_config == null)
                {
                    LoadConfig();
                }
                return _config;
            }
        }
        
        public static void LoadConfig()
        {
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    string json = File.ReadAllText(ConfigFilePath);
                    _config = JsonSerializer.Deserialize<AppConfig>(json);
                }
                else
                {
                    // 如果配置文件不存在，创建默认配置
                    _config = new AppConfig();
                    SaveConfig();
                }
            }
            catch (Exception ex)
            {
                // 如果加载配置失败，使用默认配置
                _config = new AppConfig();
                Console.WriteLine($"加载配置文件失败: {ex.Message}");
            }
        }
        
        public static void SaveConfig()
        {
            try
            {
                string json = JsonSerializer.Serialize(_config, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ConfigFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存配置文件失败: {ex.Message}");
            }
        }
        
        public static void UpdateConnectionString(string connectionString)
        {
            Config.ConnectionString = connectionString;
            SaveConfig();
        }
        
        public static void UpdateLogConfig(LogConfig logConfig)
        {
            Config.LogConfig = logConfig;
            SaveConfig();
        }
        
        public static void UpdateSystemInfo(string systemName, string version, string companyName, string companyLogo)
        {
            Config.SystemName = systemName;
            Config.Version = version;
            Config.CompanyName = companyName;
            Config.CompanyLogo = companyLogo;
            SaveConfig();
        }
        
        public static void AddOrUpdateCustomSetting(string key, string value)
        {
            if (Config.CustomSettings.ContainsKey(key))
            {
                Config.CustomSettings[key] = value;
            }
            else
            {
                Config.CustomSettings.Add(key, value);
            }
            SaveConfig();
        }
        
        public static string GetCustomSetting(string key, string defaultValue = null)
        {
            if (Config.CustomSettings.TryGetValue(key, out string value))
            {
                return value;
            }
            return defaultValue;
        }
    }
} 